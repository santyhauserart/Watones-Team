using System.Collections.Generic;

public static class Inventory
#region Qué hace el script
//Este script es la mochila del juego: guarda qué cosas tenés y cuántas de cada una.
//Sabe meter una cosa (Add), preguntar cuántas hay (Count) y sacar una (Remove).
//public: otros scripts lo pueden usar (ItemPickup y CombatMenu lo usan).
//static: es uno solo para todo el juego, como un pizarrón en la pared del aula. No se pega a ningún objeto ni hay que crearlo, y no se borra al cambiar de escena.
#endregion
{
    private static Dictionary<string, int> items = new Dictionary<string, int>();
    #region El cuaderno de la mochila
    //Es el cuaderno donde se anota qué cosas hay y cuántas. Cada renglón dice: nombre de la cosa → cantidad.
    //Ejemplo: "Potion" → 2, o sea, 2 pociones.
    //private: nadie de afuera lo toca directo, solo con las funciones de abajo. static: es uno solo para todo el juego.
    //Dictionary<string, int>: un diccionario. La primera parte (string, un texto) es el nombre de la cosa y la segunda (int, un número entero) es cuántas hay.
    //= new Dictionary<string, int>(): "creá un cuaderno nuevo, vacío". Sin esto el cuaderno no existe y da error al usarlo.
    #endregion
    public static void Add(string itemName, int amount = 1)
    #region Meter algo en la mochila
    //Es la función para meter una cosa en la mochila.
    //public: otros scripts la pueden llamar (ItemPickup la usa al agarrar la poción).
    //static: se llama directo con Inventory.Add(...), sin crear nada.
    //string itemName: hay que decirle cómo se llama la cosa, con letras. Ejemplo: "Potion".
    //int amount = 1: cuántas mete. Si no le decís nada, mete 1. Por eso Inventory.Add("Potion") mete una sola.
    #endregion
    {
        if (items.ContainsKey(itemName))
        #region ¿Ya tengo una página?
        //Pregunta: "¿ya existe en el cuaderno una página con este nombre?".
        #endregion
        {
            items[itemName] += amount;
            #region Sumar a la página
            //Como ya tenés de esa cosa, le suma la cantidad nueva a la página que ya existe.
            #endregion
        }
        else
        {
            items[itemName] = amount;
            #region Abrir una página nueva
            //Abre una página nueva en el cuaderno con esa cantidad.
            //Ejemplo: no había nada, agarrás una poción y queda Potion → 1.
            //Si siempre se usara "=" en vez de "+=", cada poción nueva borraría el número anterior y siempre tendrías 1.
            #endregion

        }
        UnityEngine.Debug.Log($"Agarraste: {itemName}. Ahora tenés {Count(itemName)}");
        #region Avisar qué agarraste
        //Escribe un mensaje en la Console, solo para que mires vos.
        //UnityEngine.Debug.Log(...): escribir en la Console. Se escribe UnityEngine completo porque arriba no hay "using UnityEngine".
        //$"...": un texto donde lo que va entre llaves { } se reemplaza por su valor.
        //{itemName}: el nombre de la cosa. {Count(itemName)}: pregunta cuántas hay ahora, usando la función de más abajo.
        //Ejemplo: "Agarraste: Potion. Ahora tenés 2".
        #endregion
    }

    public static int Count(string itemName)
    #region Preguntar cuántas hay
    //Es la función para preguntar cuántas hay de una cosa.
    //public: otros scripts la pueden llamar. static: se llama directo con Inventory.Count(...).
    //int: devuelve un número entero (la cantidad).
    //Count: es el nombre ("contar").
    //string itemName: hay que decirle de qué cosa preguntás.
    #endregion
    {
        return items.ContainsKey(itemName) ? items[itemName] : 0;
        #region Responder la cantidad
        //Devuelve la cantidad de esa cosa: si está en el cuaderno, el número que tiene; si no, 0.
        //return: "devolvé esta respuesta a quien me preguntó".
        //items.ContainsKey(itemName): "¿existe la página?". Da sí o no.
        //?: significa "entonces...". Mira la respuesta de la pregunta de antes y elige un camino.
        //items[itemName]: el número de la página. Es el camino si la respuesta fue sí.
        //:: significa "si no...". Separa los dos caminos. 0: el número que devuelve si la respuesta fue no.
        //Si le pidieras items["Espada"] y esa página no existe, Unity daría error. Por eso primero se pregunta si existe.
        //Esta forma de escribirlo se llama "operador ternario".
        #endregion
    }

    public static bool Remove(string itemName)
    #region Sacar una de la mochila
    //Es la función para sacar una cosa de la mochila.
    //public: otros scripts la pueden llamar (CombatMenu la usa al usar la poción). static: se llama directo con Inventory.Remove(...).
    //bool: devuelve sí o no (true o false). Dice true si pudo sacar y false si no tenías ninguna.
    //Remove: es el nombre ("sacar").
    //string itemName: hay que decirle qué cosa sacar.
    #endregion
    {
        if (Count(itemName) <= 0) return false;
        #region ¿No tengo ninguna?
        //Pregunta: "¿tengo cero (o menos) de esta cosa?". Si sí, responde "no" y se termina la función acá.
        //if: "si pasa esto...". Count(itemName): le pregunta a la función de arriba cuántas hay.
        //return false;: "devolvé no y cortá acá". Lo que está abajo no se ejecuta.
        //Sin llaves: el if solo manda a esa parte de return.
        //Así nunca podrías tener "-1 pociones".
        #endregion
        items[itemName] -= 1;
        #region Sacar una
        //Resta una a la página de esa cosa en el cuaderno.
        //items[itemName]: la página de esa cosa. -=: significa "restale". 1: uno.
        //Ejemplo: Potion → 2 pasa a Potion → 1.
        #endregion
        UnityEngine.Debug.Log("Usaste: " + itemName + ". Te quedan " + Count(itemName));
        return true;
        #region Responder que sí
        //Devuelve "sí" (true): pude sacar una.
        //Quien llamó a Remove usa esta respuesta para decidir qué hacer. Por ejemplo, CombatMenu cura al jugador solo si da true.
        #endregion
    }
}
