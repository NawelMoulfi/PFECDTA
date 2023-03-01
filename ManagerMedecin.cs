using UnityEngine;
using System.Data;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEditor;
//using MySql.Data.MySqlClient;


public class ManagerMedecin
{
    private IDbConnection dbConnection;
    //private MySqlConnection dbConnection;

    
    public ManagerMedecin(IDbConnection db)
    {
        this.dbConnection = db;
    } 
    /*
    public ManagerMedecin(MySqlConnection db)
    {
        this.dbConnection = db;
    }
    */
    /*
    public void getMedecinByEmailAndMdp(string email, string mdp)
    {
        Medecin medecin = null;

        MySqlCommand command = this.dbConnection.CreateCommand();
        command.CommandText = "Select * from news where id = 3";
        this.dbConnection.Open();
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            MonoBehaviour.print("titre = " + reader["titre"].ToString());
        }  
        
        //return medecin;
    }
    */
    
    public Medecin getMedecinByEmailAndMdp(string email, string mdp)
    {
        Medecin medecin = null;
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = this.dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("Select * from  Medecin where EmailM ='" + email + 
                    "'and MdpM = '" + mdp + "'");

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                       // medecin = new Medecin(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), 
                          //  reader.GetString(3), reader.GetString(4),reader.GetInt32(5),
                          //   reader.GetString(6), reader.GetString(7), reader.GetString(8), reader.GetString(9));
                        medecin = new Medecin(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                            reader.GetString(3), reader.GetString(4), reader.GetInt32(5),
                             reader.GetString(6), reader.GetString(7), reader.GetString(8));
                    }
                }

                this.dbConnection.Close();

            }
        }
        return medecin;
    }

    public Medecin getMedecinByEmail(string email)
    {
        Medecin medecin = null;
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = this.dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("Select * from  Medecin where EmailM ='" + email + "'");

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        medecin = new Medecin(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                            reader.GetString(3), reader.GetString(4), reader.GetInt32(5),
                             reader.GetString(6), reader.GetString(7), reader.GetString(8), reader.GetString(9));
                    }
                }

                this.dbConnection.Close();

            }
        }
        return medecin;
    }

    public Medecin getMedecinById(int id)
    {
        Medecin medecin = null;
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = this.dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("Select * from  Medecin where IdM =" + id);

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        medecin = new Medecin(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                            reader.GetString(3), reader.GetString(4), reader.GetInt32(5),
                             reader.GetString(6), reader.GetString(7), reader.GetString(8), reader.GetString(9));
                    }
                }

                this.dbConnection.Close();

            }
        }
        return medecin;
    }

    public List <Medecin> getAllMedecin()
    {
        List<Medecin> medecins = new List<Medecin>();
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = this.dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("Select * from  Medecin ");

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        Medecin medecin = new Medecin(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                            reader.GetString(3), reader.GetString(4), reader.GetInt32(5),
                             reader.GetString(6), reader.GetString(7), reader.GetString(8), reader.GetString(9));
                        i++;

                        medecins.Add(medecin);
                    }
                }
                return medecins;
            }  
        }
    }

    public void addMedecin(Medecin medecin)
    {
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = this.dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("INSERT into Medecin VALUES(null, '" + medecin.nomM + 
                    "', '" + medecin.prenomM + "', '" + medecin.dateNaissM + "', '" + medecin.sexeM + "', '" + 
                    medecin.numTelM + "', '" + medecin.adresseM + "', '" + medecin.emailM + "', '" + medecin.mdpM + "', '" + medecin.imageM + "' )");

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();


                dbConnection.Close();
            }
        }

    }

    public void updateMedecin(Medecin medecin)
    {
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("Update Medecin set NomM = '" + medecin.nomM
                                                + "', PrenomM = '" + medecin.prenomM
                                                + "',DateNaisM =  '" + medecin.dateNaissM
                                                + "', SexeM = '" + medecin.sexeM
                                                + "',NumTelM = '" + medecin.numTelM
                                                + "',AdresseM = '" + medecin.adresseM
                                                + "',EmailM = '" + medecin.emailM
                                                + "',MdpM = '" + medecin.mdpM
                                                + "',imageM = '" + medecin.imageM
                                                + "' where IdM = " + medecin.idM);

                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }

    }

    public void deleteMedecin(int ID)
    {
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = this.dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("delete from Medecin where idM = '" + ID + "'");
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }

    }

}

