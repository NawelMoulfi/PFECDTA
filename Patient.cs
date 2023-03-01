using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;



public class Patient
{
    public int idP;
    public string nomP;
    public string prenomP;
    public string dateNaissP;
    public string sexeP;
    public int numTelP;
    public int idMedecin;
    public string imageP;


    public Patient()
    {

    }


    public Patient(string nom, string prenom, string dateNaissance, string sexe, int tel, int medecin, string image)
    {

        this.nomP = nom;
        this.prenomP = prenom;
        this.dateNaissP = dateNaissance;
        this.sexeP = sexe;
        this.numTelP = tel;
        this.idMedecin = medecin;
        this.imageP = image;
    }

    public Patient(int id, string nom, string prenom, string dateNaissance, string sexe, int tel, int medecin, string image)
    {
        this.idP = id;
        this.nomP = nom;
        this.prenomP = prenom;
        this.dateNaissP = dateNaissance;
        this.sexeP = sexe;
        this.numTelP = tel;
        this.idMedecin = medecin;
        this.imageP = image;
    }
}
    
