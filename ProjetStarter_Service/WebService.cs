using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoucheDonnee_MySQL;
using MySql.Data.MySqlClient;
using ProjetStarter_Service;

public class WebService
{
    string ConnectionString = "Server=10.76.10.165;Database=db_dash;Uid=scopelg;Pwd=Upsa-dash0init!;";



    public List<Tasks> GetTaches(string nomS)
    {
        List<Tasks> LesTasks = new List<Tasks>();
        List<ParamSQL> Params = new List<ParamSQL>();
        sql lnk = new sql(ConnectionString);
        try
        {
            Params.Add(new ParamSQL("idS", nomS));
            string requete = "SELECT t.nomTache as 'nom', a.service,t.frequence, t.idTaches, a.ip   " +
            "from Taches t, api a " +
            "where t.idService = a.id " +
            "and a.service = @idS " +
            " group by t.nomTache,t.frequence ,a.service,t.idTaches, a.ip ";

            MySqlDataReader Reader;
            Reader = lnk.RequeteSELECT_Generic(requete, Params);
            while (Reader.Read())
            {
                LesTasks.Add(new Tasks(Reader.GetString(0), Reader.GetString(1), Reader.GetInt32(2), Reader.GetInt32(3), Reader.GetString(4)));
            }
        }
        catch (Exception e)
        {
            lnk.AddErreur("GetTaches", "Service : " + nomS, e);
        }
        finally
        {
            lnk.CloseSQL();
        }

        return LesTasks;
    }


    public void AddErreur(Exception ex)
    {
        sql lnk = new sql(ConnectionString);

        lnk.AddErreur("", "", ex);

        lnk.CloseSQL();
    }

    public bool InsertNewDashboard(string statutvalue, int idtache, float valeur)
    {
        bool result = false;
        sql lnk = new sql(ConnectionString);
        List<ParamSQL> Params = new List<ParamSQL>();
        string requete;

        try
        {
            
            Params.Add(ParamSQL.CustomParameter("time", DateTime.Now, "DATETIME"));
            Params.Add(new ParamSQL("nomService", "Local"));
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
}
