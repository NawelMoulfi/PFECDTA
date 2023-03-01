using UnityEngine;
using System.Collections;

public class Examen
{
    public int idE;
    public string dateE;
    public int idPatient;
    public string chemin;
    public string note;

    public Examen()
    {

    }


    public Examen(string date, int patient, string chemin, string note)
    {
        this.dateE = date;
        this.idPatient = patient;
        this.chemin = chemin;
        this.note = note;
    }

    public Examen(int id, string date, int patient, string chemin, string note)
    {
        this.idE = id;
        this.dateE = date;
        this.idPatient = patient;
        this.chemin = chemin;
        this.note = note;
    }
}
