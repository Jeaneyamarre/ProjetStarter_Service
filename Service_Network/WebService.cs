using System;
using System.Collections.Generic;
using CoucheDonnee_MySQL;
using MySql.Data.MySqlClient;
using Service_Network;

public class WebService
{
    string ConnectionString = "Server=10.76.10.165;Database=db_dash;Uid=scopelg;Pwd=Upsa-dash0init!;";

    public List<ServiceStatut> GetAllDashboard()
    {
        List<ServiceStatut> LesDashboard = new List<ServiceStatut>();
        sql lnk = new sql(ConnectionString);
        try
        {
            string requete = "SELECT  l.nomService as 'nom' ,l.statut as 'statut' ,max(l.time) as 'lastUpdate', idTache  " +
             "from logs l " +
             "group by l.nomService, l.statut,idTache;";
            MySqlDataReader Reader;
            Reader = lnk.RequeteSELECT_Generic(requete);
            while (Reader.Read())
            {
                LesDashboard.Add(new ServiceStatut(Reader.GetString(0), Reader.GetString(1), Reader.GetString(2), Reader.GetString(3)));
            }
        }
        catch (Exception e)
        {
            lnk.AddErreur("GetAllDashboard", "*", e);
        }
        finally
        {
            lnk.CloseSQL();
        }

        return LesDashboard;
    }

    public List<ServiceStatut> GetDashboard_ById(string idDashboard)
    {
        List<ServiceStatut> LeService = new List<ServiceStatut>();
        List<ParamSQL> Params = new List<ParamSQL>();
        sql lnk = new sql(ConnectionString);
        try
        {

            //Params.Add(new ParamSQL("idD", idDashboard));
            string requete = "SELECT l.nomService as 'nom' ,l.statut as 'statut' ,max(l.time) as 'lastUpdate',idTache  " +
            "from logs l  " +
            "where l.nomService ='LabVantage' " +
            " group by l.nomService, l.statut,idTache ;";

            MySqlDataReader Reader;
            Reader = lnk.RequeteSELECT_Generic(requete);
            while (Reader.Read())
            {
                LeService.Add(new ServiceStatut(Reader.GetString(0), Reader.GetString(1), Reader.GetString(2), Reader.GetString(3)));
            }
        }
        catch (Exception e)
        {
            lnk.AddErreur("GetDashboard_ById", "ID : " + idDashboard, e);
        }
        finally
        {
            lnk.CloseSQL();
        }

        return LeService;
    }

    public List<Tasks> GetTaches()
    {
        List<Tasks> LesTasks = new List<Tasks>();
        List<ParamSQL> Params = new List<ParamSQL>();
        sql lnk = new sql(ConnectionString);
        try
        {
            string requete = "SELECT t.nomTache as 'nom', a.service,t.frequence, t.idTaches, a.ip, a.dns   " +
            "from Taches t, api a " +
            "where t.idService = a.id " +
            " group by t.nomTache,t.frequence ,a.service,t.idTaches, a.ip ";

            MySqlDataReader Reader;
            Reader = lnk.RequeteSELECT_Generic(requete, Params);
            while (Reader.Read())
            {
                LesTasks.Add(new Tasks(Reader.GetString(0), Reader.GetString(1), Reader.GetInt32(2), Reader.GetInt32(3), Reader.GetString(4), Reader.GetString(5)));
            }
        }
        catch (Exception e)
        {
            lnk.AddErreur("GetTaches", "Service : " , e);
        }
        finally
        {
            lnk.CloseSQL();
        }

        return LesTasks;
    }


    public bool InsertNewDashboard(string statutvalue, int idtache, float valeur,string nomservice)
    {
        bool result = false;
        sql lnk = new sql(ConnectionString);
        List<ParamSQL> Params = new List<ParamSQL>();
        string requete;
        try
        {
            Params.Add(ParamSQL.CustomParameter("time", DateTime.Now, "DATETIME"));
            Params.Add(new ParamSQL("nomService", nomservice));
            Params.Add(new ParamSQL("statut", statutvalue));
            Params.Add(new ParamSQL("idTache", idtache.ToString()));
            Params.Add(new ParamSQL("valeur", valeur.ToString(".##")));
            requete = "INSERT INTO logs(time,nomService, statut,idTache,value) VALUES(@time, @nomService, @statut,@idTache,@valeur)";
            if (lnk.RequeteINSERTDELETE(requete, Params) > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
        }
        catch (Exception e)
        {
            lnk.AddErreur("InsertNewDashboard", statutvalue, e);
            result = false;
        }
        finally
        {
            lnk.CloseSQL();
        }

        return result;
    }

    public bool DeleteOldlogs()
    {
        bool result = false;
        sql lnk = new sql(ConnectionString);
        List<ParamSQL> Params = new List<ParamSQL>();
        string requete;
        try
        {
            requete = "Delete from logs where datediff(CURDATE(),time)>7";
            if (lnk.RequeteINSERTDELETE(requete, Params) > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
        }
        catch (Exception e)
        {
            lnk.AddErreur("DeleteOldlogs", "", e);
            result = false;
        }
        finally
        {
            lnk.CloseSQL();
        }

        return result;
    }

    public void AddErreur(Exception ex)
    {
        sql lnk = new sql(ConnectionString);

        lnk.AddErreur("", "", ex);

        lnk.CloseSQL();
    }
}
