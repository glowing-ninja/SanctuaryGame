
// Require a character controller to be attached to the same game object
@script RequireComponent(CharacterController)


enum CharacterState {
	Idle = 0,
	Walking = 1
}

private var _characterState : CharacterState;

// The speed when walking
var walkSpeed = 2.0;
// The gravity for the character
var gravity = 20.0;
// The gravity in controlled descent mode
var speedSmoothing = 10.0;
var rotateSpeed = 500.0;

// The current move direction in x-z
private var moveDirection = Vector3.zero;
// The current x-z move speed
private var moveSpeed = 0.0;

// The last collision flags returned from controller.Move
private var collisionFlags : CollisionFlags; 

// Are we moving backwards (This locks the camera to not do a 180 degree spin)
private var movingBack = false;
// Is the user pressing any keys?
private var isMoving = false;
// When did the user start walking (Used for going into trot after a while)
private var walkTimeStart = 0.0;

private var isControllable = true;

function Awake ()
{
	if (GetComponent.<NetworkView>().isMine) {
		moveDirection = transform.TransformDirection(Vector3.forward);	
	}
	else 
		enabled = false;
}


function UpdateSmoothedMovementDirection ()
{
	var cameraTransform = Camera.main.transform;
	
	// Forward vector relative to the camera along the x-z plane	
	var forward = cameraTransform.TransformDirection(Vector3.forward);
	forward.y = 0;
	forward = forward.normalized;

	// Right vector relative to the camera
	// Always orthogonal to the forward vector
	var right = Vector3(forward.z, 0, -forward.x);

	var v = Input.GetAxisRaw("Vertical");
	var h = Input.GetAxisRaw("Horizontal");

	// Are we moving backwards or looking backwards
	if (v < -0.2)
		movingBack = true;
	else
		movingBack = false;
	
	isMoving = Mathf.Abs (h) > 0.1 || Mathf.Abs (v) > 0.1;
		
	// Target direction relative to the camera
	var targetDirection = h * right + v * forward;

		// We store speed and direction seperately,
		// so that when the character stands still we still have a valid forward direction
		// moveDirection is always normalized, and we only update it if there is user input.
		if (targetDirection != Vector3.zero)
		{
			// If we are really slow, just snap to the target direction
			if (moveSpeed < walkSpeed * 0.9)
			{
				moveDirection = targetDirection.normalized;
			}
			// Otherwise smoothly turn towards it
			else
			{
				moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
				
				moveDirection = moveDirection.normalized;
			}
		}
		
		// Smooth the speed based on the current target direction
		var curSmooth = speedSmoothing * Time.deltaTime;
		
		// Choose target speed
		//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
		var targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0);
	
		_characterState = CharacterState.Idle;
	
		targetSpeed *= walkSpeed;
		_characterState = CharacterState.Walking;
		
		moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
		
		// Reset walk time start when we slow down
		if (moveSpeed < walkSpeed * 0.3)
			walkTimeStart = Time.time;
	

		
}




function Update() {
	
	if (!isControllable)
	{
		// kill all inputs if not controllable.
		Input.ResetInputAxes();
	}

	UpdateSmoothedMovementDirection();
	
	
	// Calculate actual motion
	var movement = moveDirection * moveSpeed;
	movement *= Time.deltaTime;
	
	// Move the controller
	var controller : CharacterController = GetComponent(CharacterController);
	collisionFlags = controller.Move(movement);
	
	
	
	// Set rotation to the move direction
	/////////transform.rotation = Quaternion.LookRotation(moveDirection);
}

function OnControllerColliderHit (hit : ControllerColliderHit )
{
//	Debug.DrawRay(hit.point, hit.normal);
	if (hit.moveDirection.y > 0.01) 
		return;
}

function GetSpeed () {
	return moveSpeed;
}

function GetDirection () {
	return moveDirection;
}

function IsMovingBackwards () {
	return movingBack;
}


function IsMoving ()  : boolean
{
	return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5;
}


function Reset ()
{
	gameObject.tag = "Player";
}

