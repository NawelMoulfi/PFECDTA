using UnityEngine;
using System.Collections;
using System.Data;
using System.Collections.Generic;

public class ManagerExamen
{

    private IDbConnection dbConnection;
    //private MySqlConnection dbConnection;


    public ManagerExamen(IDbConnection db)
    {
        this.dbConnection = db;
    }

    public Examen getExamenById(int id)
    {
        Examen examen = null;
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = this.dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("Select * from  Examen where IdE =" + id);

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        examen = new Examen(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4));
                    }
                }

                this.dbConnection.Close();

            }
        }
        return examen;
    }

    public List<Examen> getExamenByPatient(int idP)
    {
        List<Examen> examens = new List<Examen>();
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = this.dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("Select * from  Examen where IdPatient =" + idP);

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        Examen examen = new Examen(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4));
                        i++;
                        examens.Add(examen);
                    }
                }
                return examens;
            }
        }
    }

    public List<Examen> getAllExamen()
    {
        List<Examen> examens = new List<Examen>();
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = this.dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("Select * from  Examen ");

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        Examen examen = new Examen(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4));
                        i++;
                        examens.Add(examen);
                    }
                }
                return examens;
            }
        }
    }

    public void addExamen(Examen examen)
    {
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = this.dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("INSERT into Examen VALUES(null, '" + examen.dateE +
                    "', '" + examen.idPatient + "', '" + examen.chemin + "', '" + examen.note + "')");

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();


                dbConnection.Close();
            }
        }

    }

    public void updateExamen(Examen examen)
    {
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("Update Examen set DateE = '" + examen.dateE
                                                + "',IdPatient = '" + examen.idPatient
                                                + "',Note = '" + examen.note
                                                + "',Chemin = '" + examen.chemin
                                                + "' where IdE = " + examen.idE);

                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }

    }

    public void deleteExamen(int ID)
    {
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = this.dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("delete from Examen where IdE = '" + ID + "'");
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }

    }

    public string getNextId()
    {
        string id = "1";
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = this.dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("select IdE from Examen ORDER BY IdE DESC ");
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();

                Debug.Log("deb");
                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    if (reader.Read()) id = (reader.GetInt32(0) + 1).ToString();
                }
                Debug.Log("fin id = " + id);

                dbConnection.Close();
            }
        }
        return id;
    }

}
