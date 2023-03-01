using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
//using System.Windows;
//using MySql.Data.MySqlClient;

public class ConnexionDB
{
    private static string cheminDB;
    private static IDbConnection dbConnection;
    //private static MySqlConnection dbConnection;

    public ConnexionDB()
    {

    }
    
    public static IDbConnection DB()
    {
        ConnexionDB.cheminDB = "URI=file:" + Application.dataPath + "/Db/my_dataset.db";
        ConnexionDB.dbConnection = new SqliteConnection(ConnexionDB.cheminDB);
        return ConnexionDB.dbConnection;
    }
    
    /*
    public static MySqlConnection DB()
    {
        ConnexionDB.cheminDB = "Server=localhost;Port=80;Database=test;username=root;password=";
        ConnexionDB.dbConnection = new MySqlConnection(ConnexionDB.cheminDB);
        return ConnexionDB.dbConnection;
    }
    */

}
