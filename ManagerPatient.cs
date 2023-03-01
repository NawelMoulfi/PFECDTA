using UnityEngine;
using System.Data;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEditor;
//using MySql.Data.MySqlClient;


public class ManagerPatient
{
    private IDbConnection dbConnection;
    //private MySqlConnection dbConnection;


    public ManagerPatient(IDbConnection db)
    {
        this.dbConnection = db;
    }

    public Patient getPatientById(int id)
    {
        Patient patient = null;
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = this.dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("Select * from  Patient where IdP =" + id);

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patient = new Patient(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                            reader.GetString(3), reader.GetString(4), reader.GetInt32(5),
                             reader.GetInt32(6), reader.GetString(7));
                    }
                }

                this.dbConnection.Close();

            }
        }
        return patient;
    }

    public List<Patient> getPatientByMedecin(int idM)
    {
        List<Patient> patients = new List<Patient>();
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = this.dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("Select * from  Patient where IdMedecin =" + idM);

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Patient patient = new Patient(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                            reader.GetString(3), reader.GetString(4), reader.GetInt32(5),
                             reader.GetInt32(6), reader.GetString(7));

                        patients.Add(patient);
                    }
                }

                this.dbConnection.Close();

            }
        }
        return patients;
    }

    public List<Patient> getAllPatient()
    {
        List<Patient> patients = new List<Patient>();
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = this.dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("Select * from  Patient ");

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        Patient patient = new Patient(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                            reader.GetString(3), reader.GetString(4), reader.GetInt32(5),
                             reader.GetInt32(6), reader.GetString(7));
                        i++;

                        patients.Add(patient);
                    }
                }
                return patients;
            }
        }
    }

    public void addPatient(Patient patient)
    {
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = this.dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("INSERT into Patient VALUES(null, '" + patient.nomP +
                    "', '" + patient.prenomP + "', '" + patient.dateNaissP + "', '" + patient.sexeP + "', '" +
                    patient.numTelP + "','"+ patient.idMedecin + "','"+ patient.imageP + "')");

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();


                dbConnection.Close();
            }
        }

    }

    public void updatePatient(Patient patient)
    {
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("Update Patient set NomP = '" + patient.nomP
                                                + "', PrenomP = '" + patient.prenomP
                                                + "',DateNaisP =  '" + patient.dateNaissP
                                                + "', SexeP = '" + patient.sexeP
                                                + "',NumTelP = '" + patient.numTelP
                                                + "',IdMedecin = '" + patient.idMedecin
                                                + "',imageP = '" + patient.imageP
                                                + "' where IdP = " + patient.idP);

                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }

    }

    public void deletePatient(int ID)
    {
        using (this.dbConnection)
        {
            this.dbConnection.Open();
            using (IDbCommand dbCmd = this.dbConnection.CreateCommand())
            {
                string sqlQuery = string.Format("delete from Patient where idP = '" + ID + "'");
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }

    }

}

