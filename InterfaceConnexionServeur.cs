using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

public class InterfaceConnexionServeur : MonoBehaviour
{
    public GameObject interfaceAccueil;
    public GameObject interfaceConnexionSeveur;
    public GameObject panelConnexionServeur;
	public GameObject accueilConnexionServeur;
	public GameObject champs;
    public GameObject interfaceMedecin;
    public GameObject interfaceOperateur;
    public InputField email;
    public InputField motDePasse;
    public GameObject messageErreur;
    public GameObject interfaceNewMdp;
    public InputField emailConfirme;
    public InputField newMdp;
    public InputField newMdp2;
    public GameObject emailErreur;
    public GameObject mdpErreur;



    // Start is called before the first frame update
    public void starting()
    {
        //interfaceConnexionSeveur.SetActive(true);
		panelConnexionServeur.SetActive(true);
		accueilConnexionServeur.SetActive (true);
		champs.SetActive (true);
        interfaceNewMdp.SetActive(false);
        messageErreur.SetActive(false);
        interfaceMedecin.SetActive(false);
        interfaceOperateur.SetActive(false);
        //test();
    }

    public void Connexion_Menu()
    {
        string EmailConnexion = email.text;
        string MotDePasseConnexion = motDePasse.text;
        
        //Debug.Log("EmailConnexion = " + EmailConnexion);
        //Debug.Log("MotDePasseConnexion = " + MotDePasseConnexion);
        
        if ((EmailConnexion == "") && ( MotDePasseConnexion == "") || ((EmailConnexion == "Admin") && (MotDePasseConnexion == "Admin")))
        {
			panelConnexionServeur.SetActive(false);
            messageErreur.SetActive(false);
            interfaceMedecin.SetActive(false);
            interfaceOperateur.SetActive(true);
            email.text = "";
            motDePasse.text = "";
			interfaceOperateur.GetComponent<InterfaceOperateur> ().starting ();
        }
        else
        {
            ManagerMedecin managerMedecin = new ManagerMedecin(ConnexionDB.DB());
            Medecin medecin = managerMedecin.getMedecinByEmailAndMdp(EmailConnexion, MotDePasseConnexion);
            if (medecin != null)
            {
                SessionState.SetString("SessionIdMedecin", medecin.idM.ToString());
				panelConnexionServeur.SetActive(false);
                messageErreur.SetActive(false);
                interfaceMedecin.SetActive(true);
                interfaceOperateur.SetActive(false);
                email.text = "";
                motDePasse.text = "";
				interfaceMedecin.GetComponent<InterfaceMedecin> ().starting ();
            }
            else 
            {
                messageErreur.SetActive(true);
            }
        }

    }

    public void mdpOublier()
    {
		email.text = "";
		motDePasse.text = "";
    }

    public void updateMdp()
    {
        string email = emailConfirme.text;
        string motDePasse = newMdp.text;
        string motDePasseConf = newMdp2.text;

        ManagerMedecin managerMedecin = new ManagerMedecin(ConnexionDB.DB());
        Medecin medecin = managerMedecin.getMedecinByEmail(email);

        emailErreur.SetActive(false);
        mdpErreur.SetActive(false);
        if (medecin != null)
        {
            if (motDePasse.Equals(motDePasseConf))
            {
                medecin.mdpM = motDePasse;
                managerMedecin.updateMedecin(medecin);
                interfaceNewMdp.SetActive(false);
                champs.SetActive(true);
            }
            else
            {
                mdpErreur.SetActive(true);
                //interfaceNouveauMdp.SetActive(true);
                //interfaceConnexion.SetActive(false);
                newMdp.text = "";
                newMdp2.text = "";
            }
        }
        else
        {

            emailErreur.SetActive(true);
            //MDPErreur.SetActive(false);
            //interfaceNouveauMdp.SetActive(true);
            //interfaceConnexion.SetActive(false);
            emailConfirme.text = "";
            newMdp.text = "";
            newMdp2.text = "";
        }
    }

	public void retour()
	{
		email.text = "";
		motDePasse.text = "";
	}

	public void annuler()
	{
		emailConfirme.text = "";
		newMdp.text = "";
		newMdp2.text = "";
	}

}
