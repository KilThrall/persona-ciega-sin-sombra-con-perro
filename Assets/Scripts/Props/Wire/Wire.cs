using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool isGrabbedByPlayer;

    private GenericInteractionTrigger trigger;
    private ItemSocket itemSocket;
    [SerializeField]
    private Wire wireToSplice;

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        trigger = GetComponent<GenericInteractionTrigger>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLUG_TAG))
        {
            itemSocket = collision.GetComponent<ItemSocket>();
        }
        if (collision.CompareTag(WIRE_TAG))
        {
            print("me voy a enganchar al cable");
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
            print($"WireToSplice is null?: {wireToSplice==null}");
            if (itemSocket == null && wireToSplice == null) //SI EL WIRE NO ESTÁ CON COLISION CON OTRO WIRE NI CON UN ITEM SOCKET
            { // SE SUELTA
                DropWire();
                rope.DropRope();
            }
            else if (itemSocket != null) // SI HAY ITEM SOCKET:
            { // SE CONECTA Y SE LLAMA LA CONECCION
                DropWire();
                ConnectRope(itemSocket.transform);
                itemSocket.Connect();
            }//GUARDA PORQUE SE PUEDE DESCONECTAR Y ESO NO HACE NADA PERO TAMPOCO ERA UN REQUISITO Q NO PASE NADA
            
            else if (wireToSplice != null) //Si hay un Wire al que se pueda conectar se conecta, al estar conectado ya no se podrá agarrar y se suelta automaticamente
            {
                ConnectRope(wireToSplice.transform);
                isSpliced = true;
                DropWire();
            }
        }
        else 
        {
            print($"WireToSplice is null when not grabbed?: {wireToSplice == null}");
            if (!isSpliced) // osea si no esta agarrado ni empalmado
            {
                GrabWire();
            }
            if (wireToSplice != null) // si se toca la E en este Wire se va a conectar al otro que entró en colision
            {
                ConnectRope(wireToSplice.transform);
                isSpliced = true;
                DropWire();
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
}