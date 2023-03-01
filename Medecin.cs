using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;



public class Medecin
{
    public int idM;
    public string nomM;
    public string prenomM;
    public string dateNaissM;
    public string sexeM;
    public int numTelM;
    public string adresseM;
    public string emailM;
    public string mdpM;
    public string imageM;


    public Medecin()
    {

    }

    public Medecin(int id,string nom, string prenom, string dateNaissance, string sexe, int tel, string adresse, string email, string motDePasse)
    {
        this.idM = id;
        this.nomM = nom;
        this.prenomM = prenom;
        this.dateNaissM = dateNaissance;
        this.sexeM = sexe;
        this.numTelM = tel;
        this.adresseM = adresse;
        this.emailM = email;
        this.mdpM = motDePasse;
        //this.imageM = image;
    }
    
    public Medecin(string nom, string prenom, string dateNaissance, string sexe, int tel, string adresse, string email, string motDePasse, string image)
    {

        this.nomM = nom;
        this.prenomM = prenom;
        this.dateNaissM = dateNaissance;
        this.sexeM = sexe;
        this.numTelM = tel;
        this.adresseM = adresse;
        this.emailM = email;
        this.mdpM = motDePasse;
        this.imageM = image;
    }
    
    public Medecin(int id, string nom, string prenom, string dateNaissance, string sexe, int tel, string adresse, string email, string motDePasse, string image)
    {
        this.idM = id;
        this.nomM = nom;
        this.prenomM = prenom;
        this.dateNaissM = dateNaissance;
        this.sexeM = sexe;
        this.numTelM = tel;
        this.adresseM = adresse;
        this.emailM = email;
        this.mdpM = motDePasse;
        this.imageM = image;

    }


}


/*
{
    
    string connectionString;

    public string id, nom, prenom, dateNaissance, sexe, tel, addresse, email, motDePasse;


    public Medecin(string nom, string prenom, string dateNaissance, string sexe, string tel, string addresse, string email, string motDePasse)
    {

        this.nom = nom;
        this.prenom = prenom;
        this.dateNaissance = dateNaissance;
        this.sexe = sexe;
        this.tel = tel;
        this.addresse = addresse;

        this.email = email;
        this.motDePasse = motDePasse;

        connectionString = "URI=file:" + Application.dataPath + "/Db/mDatabase.db";
    }
    public Medecin(string id, string nom, string prenom, string dateNaissance, string sexe, string tel, string addresse, string email, string motDePasse)
    {
        this.id = id;
        this.nom = nom;
        this.prenom = prenom;
        this.dateNaissance = dateNaissance;
        this.sexe = sexe;
        this.tel = tel;
        this.addresse = addresse;

        this.email = email;
        this.motDePasse = motDePasse;

        connectionString = "URI=file:" + Application.dataPath + "/Db/mDatabase.db";
    }


    public void AjouterMedecin()
    {

        Debug.Log("AjouterMedecin ");

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {

                cheekTableMedecin();


                string sqlQuery = string.Format("INSERT into Medecin VALUES(null, '" + nom + "', '" + prenom + "', '" + dateNaissance + "', '" + sexe + "', '" + tel + "', '" + addresse + "', '" + email + "', '" + motDePasse + "' )");

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();


                dbConnection.Close();
            }
        }

    }

    public static bool VerifierEmail(string email)
    {

        Debug.Log("VerifierEmail ");
        String connectionString = "URI=file:" + Application.dataPath + "/Db/mDatabase.db";

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {

                cheekTableMedecin();

                string sqlQuery = string.Format("Select Count(IdMedecin) from  Medecin where Email ='" + email + "'");

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();


                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int i = (int)reader.GetInt64(0);
                        if (i != 0)
                        {
                            Debug.Log(" ++++++++++++++++++++ i =" + i);
                            return true;
                        }
                    }

                }

                dbConnection.Close();

            }
        }
        return false;
    }

    public static Medecin ConnecterMedecin(string email, string motDePasse)
    {


        String connectionString = "URI=file:" + Application.dataPath + "/Db/mDatabase.db";

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {

                cheekTableMedecin();
                //Debug.Log("email = " + email);
                //Debug.Log("motDePasse = " + motDePasse);

                string sqlQuery = string.Format("Select * from  Medecin where Email ='" + email + "' and  MotDePasse = '" + motDePasse + "'");

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();


                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new Medecin("" + reader.GetInt64(0), "" + reader.GetString(1), "" + reader.GetString(2), "" + reader.GetString(3),
                            "" + reader.GetString(4), "" + reader.GetString(5), "" + reader.GetString(6), "" + reader.GetString(7), "" + reader.GetString(8));
                    }

                }

                dbConnection.Close();

            }
        }
        return null;
    }


    public static Medecin GetMedecin(string IdMedecin)
    {
        String connectionString = "URI=file:" + Application.dataPath + "/Db/mDatabase.db";

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {

                cheekTableMedecin();
                //Debug.Log("email = " + email);
                //Debug.Log("motDePasse = " + motDePasse);

                string sqlQuery = string.Format("Select * from  Medecin where IdMedecin ='" + IdMedecin + "'");

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();


                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new Medecin("" + reader.GetInt64(0), "" + reader.GetString(1), "" + reader.GetString(2), "" + reader.GetString(3),
                            "" + reader.GetString(4), "" + reader.GetString(5), "" + reader.GetString(6), "" + reader.GetString(7), "" + reader.GetString(8));
                    }

                }

                dbConnection.Close();

            }
        }
        return null;
    }


    static void cheekTableMedecin()
    {
        Debug.Log("cheekTableMedecin" + Application.dataPath);
        String connectionString = "URI=file:" + Application.dataPath + "/Db/mDatabase.db";
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQueryTable = string.Format("Create table if not exists Medecin  (" +
                   "IdMedecin  INTEGER NOT NULL UNIQUE," +
                    "NomMedecin    TEXT NOT NULL," +
                    "PrenomMedecin TEXT NOT NULL," +
                    "DateNais  TEXT NOT NULL," +
                    "Sexe  TEXT NOT NULL," +

                    "Tel   TEXT NOT NULL," +
                    "Addresse  TEXT NOT NULL," +
                    "Email  TEXT NOT NULL," +
                    "MotDePasse  TEXT NOT NULL," +
                    "PRIMARY KEY(IdMedecin AUTOINCREMENT))"
                    );
                dbCmd.CommandText = sqlQueryTable;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }

    }

    public static void DeleteTableMedcin()
    {
        String connectionString = "URI=file:" + Application.dataPath + "/Db/mDatabase.db";
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQueryTable = string.Format("drop table if exists Medecin ");
                dbCmd.CommandText = sqlQueryTable;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }



    public static void UpdateMotDePassse(string email, string motDePasse)
    {
        String connectionString = "URI=file:" + Application.dataPath + "/Db/mDatabase.db";
        Debug.Log("UpdateMotDePassse ");
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                cheekTableMedecin();
                string sqlQuery = string.Format("update  Medecin   set MotDePasse ='"+ motDePasse + "' where Email ='"+ email + "'");
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }


    public void UpdateMedecin()
    {
        cheekTableMedecin();
        Debug.Log("UpdateMedecin ");
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("Update Medecin set NomMedecin = '" + nom
                                                + "', PrenomMedecin = '" + prenom
                                                + "',DateNais =  '" + dateNaissance
                                                + "', Sexe = '" + sexe
                                                + "',Tel = '" + tel
                                                + "',Addresse = '" + addresse
                                                + "',MotDePasse = '" + motDePasse
                                                + "' where IdMedecin = " + id);
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }

    }
}


    */
