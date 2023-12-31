namespace MoogleEngine ;

using System;
using System.Text.RegularExpressions ;

public class Tools
{
    public static string Stdrize(string input)
    {
        // eliminar caracteres especiales(excepto los modificadores)
        Regex rgx = new Regex("[^a-zA-Z0-9*~!^áéíóú]");
        string filtered = rgx.Replace(input , " ") ;

        // eliminar espacios
        filtered = Regex.Replace(filtered , @"\s+" , " ");

        // Convertir a minusculas
        filtered = filtered.ToLower();

        // Devolver Resultado
        return filtered;
    }
    //-------------------------------------------------------------------------------------------------

    public static Term[] ConvertToTerms(string[] words)
    {
        // convierte la query en una lista de terminos
        Term[] output = new Term[words.Length] ;

        for (int i = 0; i < words.Length; i++)
        {
            output[i] = new Term(words[i]);            
        }
 
        return output ;
    }
    //--------------------------------------------------------------------------------------------------------
    
    public static int MaxFq(Term[] arr)
    {
        Dictionary<string , int> maxTable = new Dictionary <string , int>();

        for(int i = 0 ; i < arr.Length ; i ++)
        {
            if(! maxTable.ContainsKey(arr[i].Text))
                maxTable[arr[i].Text] = 1 ;
            
            else
                maxTable[arr[i].Text] ++ ;

        }

        return maxTable.Values.Max() ;
        
    }
    //-------------------------------------------------------------------------------------------------
    
    public static void Ordenar(Document[] docs)
    {
        // Ordena el array de docs de mayor a menor(se podria mejorar con QuickSort)
        for(int i = 0 ; i < docs.Length ; i++)
        {
            int max = i ;
            for(int j = i + 1 ; j < docs.Length ; j++)
            {
                if(docs[j].score > docs[max].score)
                    max = j;
            }
            (docs[i] , docs[max]) = (docs[max] , docs[i]);
        }
    }
    //--------------------------------------------------------------
    public static int RelevantDocs()
    {
        // Funcion que cuenta la cantidad de documentos con alguna relevancia para la query para mostrarlos
        // El maximo seria 5 y si no llega, aquellos que su score sea algo relevante.

        int max = 5 ;
        for(int i = 0 ; i < max ; i ++)
        {
            if(Dataserver.BaseDatos.docs[i].score <= 0.009 )
            {
                max = i ;
                break ;
            }
        }
        return max ;
    }
    //-------------------------------------------------------------------------------------------------
    public static int Find(string word , string[] text)
    {
        for(int j = 0 ; j < text.Length ; j++ )
        {
            if(word == Tools.Stdrize(text[j]))
                return j ;
        }
        return -1 ;
    }
    //---------------------------------------------------------------------
    public static int LevDistance(string a , string b) // sobrecarga para que se vea mas bonito el llamado
    {
        int best = int.MaxValue ;
        LevDistance(a , b , 0 , 0 , 0 , ref best) ; // llamado feo
        return best ;
    }

    public static void LevDistance(string a , string b , int i , int j , int changes , ref int best)
    {
        // calcula la distancia de edicion de 2 palabras(en un futuro agregarles podas pa que vaya mas rapido)
        if(changes > 3) {return ; }
        if(changes >= best) { return ; }

        while(i < a.Length && j < b.Length && a[i] == b[j])
        {
            i++ ;
            j++ ;
        }
        
        if( i >= a.Length || j >= b.Length - 1)
        {
            best = Math.Min( changes + (a.Length - i) + (b.Length - j) , best) ;
            return ;
        }
        LevDistance( a , b , i + 1 , j , changes + 1 , ref best) ;
        LevDistance( a , b , i , j + 1 , changes + 1 , ref best) ;
        LevDistance( a , b , i + 1 , j + 1 , changes + 1 , ref best) ;
    }       
    //----------------------------------------------------------------------
}

