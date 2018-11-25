using System;
using System.Collections.Generic;
using System.Text;
using System.configuration;
using System.Data.SqlClient;

namespace Tp05_619
{
    class ClassLogin
    {
        // grille pour le second log in 
        private  char[] Grid;
        // changer tbl user, user_anem et pass selon la bd
        private static string query1 = "select count(*) from tbluser where user_name='";
        private static string query2 = "' and pass='";
        private static string query3 = "'";
        // int qui count le nombre de tentative d'entré du mots de passe
        private int passwordatp = 0;
        // int qui compte le nombre de tentative de log int échouer
        private int loginatmp = 0;
        // paramettre qui détermine le nombre maximum de tentative pour entréer un mots de passe et ou la grille
        private int maxPassAtempt;
        // paramettre qui détermine le nombre maximum de tentative de login consécutive
        private int maxlogatempt;

        
        
        // param int row
        //param int col
        // créer un grille de dimention x, y selon les parammètes entré 
        // remplie la grille selon la méthode suivaite une lettre au hazerd + un chiffre au hazard du format A1, B3 etc
        // Retourne la grille terminé
        private char[] Newgrid(int row, int col)
        {
            Random rndchar = new Random();
            Random rndint = new Random();
            char rndLetter = rndchar.Next();
            int rndnumber = rndint.Next();
          

            Grid = new char[row, col];
            for (int x = 0; x < row ; x++)
            {
                for (int y = 0; y < col; y++)
                {
                    Grid[x, y] = rndLetter + rndnumber.tostring();
                }
            }

            return Grid;
        }
        
        
        //param userinput
        //param int row
        //param int col
        // compare l'entrer que l'utilisateur met " userinput" pour la seconde confirmation a celle de la case specifier par le 2e login 
        // représenter par row et col si le réponse est non identique, on augment la tentative de faill pass pour signaler un écheque et on retourne faux
        // si la tentative est réussie on met le nombre de tentative de pass a 0 et on retourne vrai.
        private bool gridmatch(char userinput,int row,int col )
        {

            bool match;
            if (userinput == Grid[row,col])
            {
                match = true;
              
            }
            else
            {
                match = false;
                passwordatp++;
                // mettre un message de grid fail
            }

            return match ;


        }
        //param string user
        //parame string userpass
        // établie la connection a la base de donner SQL et chercher dans la bd le User et le userpass
        // si la présence est établie on tempte un connection, si le résulta est 1 on retourne vrai et la connection est établie
        // si le résulta est 0 on retourne faux et on augment le nombre de tentative
        public bool passwordMatch(string user,string userpass )
        {
            bool match;
            if (maxlogatempt(loginatmp) =false & maxPassAtempt(passwordatp) = false)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionString["connect"].Tostring());
                con.Open();
                string query = query1 + user + query2 + userpass + query3;
                SqlCommand cmd = new SqlCommand(query, con);
                string output = cmd.ExecuteXcalar().ToSting();
                if (output == "1")
                {
                    match = true;


                }
                else
                {
                    match = false;
                    passwordatp += 1;
                    //metttre une message de pass failed
                }

            }
            else
            {
                match = false;
                // mettre une messge du nombre de tentative atteind
            }
          
           

        }

        //param maxpass
        // disponible a l'administrateur, peut mettre le nombre maximal de tentative pour un mots de pass ou un entré éroner de la grille
        public void setMaxPass(int maxpass)
        {
            maxPassAtempt = maxpass;
        }
        //param maxlog
        //disponible a l'administrateur, peut mettre le nombre maximal de tentative sucesif de connection échouer
        public void SetMaxLog(int maxlog)
        {
            maxlogatempt = maxlog;
        }
        // param resetMaxLog
        // disponible pour l'administrateur, permet de déveroullier un compte en remettant son nombre de tentative de login a zéros
        public void resetMAxLog()
        {
            loginatmp = 0;

        }
        public void resetMaxPass()
        {
            passwordatp = 0;
        }
        // param logat
        // vérifie si il reste des tentative de login avec mots de pass ou grille 
        // si les tentativeon atteind le maximum on augemnt l'écheque d'un logint de +1;
        private bool maxPassatempt(int logat)
        {
            bool atempt;
            if (logat <= maxPassAtempt)
            {
                atempt = false;
            }
            else if(logat > maxPassAtempt)
            {
                atempt = true;
                maxlogatempt++;
            }
            return atempt;
        }
        // param logat
        // vérifie si il reste des tentative de login 
        // si le nombre maximum de tentative est atteind alors on retourn faux 
        private bool maxLogAtempt(int logat)
        {

            bool atempt;
            if (logat <= maxlogatempt)
            {
                atempt = false;

            }
            else if (logat > maxlogatempt)
            {
                atempt = true;
            }
            return atempt;
        }
    }
}
