using UnityEngine;
namespace CrazyWeasol.Characters {
 public sealed class WeaselMotor : MonoBehaviour {
  [SerializeField] float runSpeed=7f,jumpForce=11f,gravity=-24f; CharacterController controller; Vector3 velocity;
  public float Speed{get;private set;} public bool IsGrounded=>controller!=null&&controller.isGrounded;
  void Awake()=>controller=GetComponent<CharacterController>();
  void Update(){if(controller==null)return;var input=new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));input=Vector3.ClampMagnitude(input,1f);var motion=transform.TransformDirection(input)*runSpeed;if(IsGrounded&&velocity.y<0)velocity.y=-2f;if(IsGrounded&&Input.GetButtonDown("Jump"))velocity.y=jumpForce;velocity.y+=gravity*Time.deltaTime;controller.Move((motion+velocity)*Time.deltaTime);Speed=new Vector3(controller.velocity.x,0,controller.velocity.z).magnitude;}
 }}
