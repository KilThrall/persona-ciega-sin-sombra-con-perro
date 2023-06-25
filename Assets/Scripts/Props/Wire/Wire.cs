using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: hacer mas prolijo este script, y el de Rope.cs

public class Wire : MonoBehaviour
{
    private const string PLUG_TAG = "Plug";
    private const string WIRE_TAG = "WireEnd";

    public bool IsGrabbedByPlayer=> isGrabbedByPlayer;
    public bool IsSpliced => isSpliced;

    #region Serialized Variables
    [SerializeField]
    private Rope rope;
    [SerializeField]
    private bool isStartTrigger;
    #endregion

    private bool isSpliced=false;
    public bool isGrabbedByPlayer;

    private BaseInteractableObject trigger;
    private ItemSocket itemSocket;
    [SerializeField]
    private Wire wireToSplice;

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        trigger = GetComponent<BaseInteractableObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLUG_TAG))
        {
            itemSocket = collision.GetComponent<ItemSocket>();
        }
        if (collision.CompareTag(WIRE_TAG))
        {
            wireToSplice = collision.GetComponent<Wire>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(PLUG_TAG))
        {
            itemSocket = null;
        }
        if (collision.CompareTag(WIRE_TAG))
        {
            wireToSplice = null;
        }
    }
    #endregion

    public void PlayerInteraction()
    {
        //comento porque no me voy a acordar lo q esta pasando aca:
        if (isGrabbedByPlayer)
        { 
            if (itemSocket == null && wireToSplice == null) //SI EL WIRE NO ESTÁ CON COLISION CON OTRO WIRE NI CON UN ITEM SOCKET
            { // SE SUELTA
                DropWire();
                rope.DropRope();

                if (isStartTrigger)
                {
                    rope.SetStartPoint(null);
                }
                else
                {
                    rope.SetEndPoint(null);
                }

            }
            else if (itemSocket != null) // SI HAY ITEM SOCKET:
            { // SE CONECTA Y SE LLAMA LA CONECCION
                DropWire();
                ConnectRope(itemSocket.transform);
                if (rope.MustBeSplicedToWork)
                {
                    if (rope.isSpliced)
                    {
                        itemSocket.Connect();
                    }
                }
                else
                {
                    itemSocket.Connect();
                }
            }//GUARDA PORQUE SE PUEDE DESCONECTAR Y ESO NO HACE NADA PERO TAMPOCO ERA UN REQUISITO Q NO PASE NADA
            
            else if (wireToSplice != null) //Si hay un Wire al que se pueda conectar se conecta, al estar conectado ya no se podrá agarrar y se suelta automaticamente
            {
                Splice();
            }
        }
        else 
        {
            if (!isSpliced) // osea si no esta agarrado ni empalmado
            {
                if (itemSocket!=null)
                {
                    itemSocket.Disconect();
                    itemSocket = null;
                }
                GrabWire();
            }
            if (!isSpliced && wireToSplice != null) // si se toca la E en este Wire se va a conectar al otro que entró en colision
            {
                Splice();
            }
        }
    }
    private void ConnectRope(Transform connectionPosition)
    {
        if (isStartTrigger)
        {
            rope.SetStartPoint(connectionPosition);
        }
        else
        {
            rope.SetEndPoint(connectionPosition);
        }
    }

    public void GrabWire()
    {
        isGrabbedByPlayer = true;
        ConnectRope(trigger.Player.transform);
    }
    public void DropWire()
    {
        isGrabbedByPlayer = false;
    }
    /// <summary>
    /// Se acopla el cable a otro
    /// </summary>
    private void Splice()
    {
        ConnectRope(wireToSplice.transform);
        isSpliced = true;
        rope.isSpliced = true;
        DropWire();
    }
}