using Assets.Scripts;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Lighsaber : MonoBehaviour
{
    //score counting init
    public TextMeshProUGUI scoreText;
    
    private static int count = 0;
    private bool countInit = false;
    //The number of vertices to create per frame
    private const int NUM_VERTICES = 12;
    public Vector3 lockedRotationEuler = new Vector3(0, 0, 0);


    [SerializeField]
    [Tooltip("The blade object")]
    private GameObject _blade = null;

    [SerializeField]
    [Tooltip("The empty game object located at the tip of the blade")]
    private GameObject _tip = null;

    [SerializeField]
    [Tooltip("The empty game object located at the base of the blade")]
    private GameObject _base = null;

    [SerializeField]
    [Tooltip("The mesh object with the mesh filter and mesh renderer")]
    private GameObject _meshParent = null;

    [SerializeField]
    [Tooltip("The number of frame that the trail should be rendered for")]
    private int _trailFrameLength = 3;

    [SerializeField]
    [ColorUsage(true, true)]
    [Tooltip("The colour of the blade and trail")]
    private Color _colour = Color.red;

    [SerializeField]
    [Tooltip("The amount of force applied to each side of a slice")]
    private float _forceAppliedToCut = 3f;

    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    private int _frameCount;
    private Vector3 _previousTipPosition;
    private Vector3 _previousBasePosition;
    private Vector3 _triggerEnterTipPosition;
    private Vector3 _triggerEnterBasePosition;
    private Vector3 _triggerExitTipPosition;
    private bool _isAttacking = false;
    private bool _isConfirm = false;
    private float _attackTimer = 0f;
    public GameObject player;
    private Vector3 offset = new Vector3(-12f, -1f, -11f); //攝影機 離 母球 相對位置
    


    // Duration of attack window (e.g., 0.5 seconds)
    [SerializeField]
    private float _attackDuration = 0.5f; // total attack window

    [SerializeField]
    private float _trailDelay = 0.2f; // time before trail appears

    [SerializeField]
    private float forwardOffset = 0.5f; // adjust how far left you want the saber


    private bool _isTrailActive = false;

    void Start()
    {
        //Init count
        if (!countInit)
        {
            count = 0;
            countInit = true;
        }
        SetCountText();
        //Init mesh and triangles
        _meshParent.transform.position = Vector3.zero;
        _mesh = new Mesh();
        _meshParent.GetComponent<MeshFilter>().mesh = _mesh;

        Material trailMaterial = Instantiate(_meshParent.GetComponent<MeshRenderer>().sharedMaterial);
        trailMaterial.SetColor("Color_8F0C0815", _colour);
        _meshParent.GetComponent<MeshRenderer>().sharedMaterial = trailMaterial;

       /* Material bladeMaterial = Instantiate(_blade.GetComponent<MeshRenderer>().sharedMaterial);
        bladeMaterial.SetColor("Color_AF2E1BB", _colour);
        _blade.GetComponent<MeshRenderer>().sharedMaterial = bladeMaterial;*/

        _vertices = new Vector3[_trailFrameLength * NUM_VERTICES];
        _triangles = new int[_vertices.Length];

        //Set starting position for tip and base
        _previousTipPosition = _tip.transform.position;
        _previousBasePosition = _base.transform.position;

    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            _isTrailActive = false;
            _attackTimer = 0f; // start timer fresh
            _isConfirm = false;

            _meshParent.SetActive(false);
            _mesh.Clear();
            _frameCount = 0;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isAttacking = true;
            _isConfirm = true; // flag to start countdown
            _isTrailActive = true;
            _meshParent.SetActive(true);
        }

        if (_isConfirm)
        {
            _attackTimer += Time.deltaTime;

            if (_attackTimer >= _attackDuration)
            {
                _isAttacking = false;
                _isTrailActive = false;
                _meshParent.SetActive(false);
                _isConfirm = false; // done
            }
        }

        if (_isAttacking)
        {
            Vector3 direction = _tip.transform.position - _base.transform.position;
            float distance = direction.magnitude;

            if (Physics.Raycast(_base.transform.position, direction.normalized, out RaycastHit hit, distance))
            {
                if (hit.collider != null && hit.collider.gameObject != null)
                {
                    AttemptSlice(hit.collider.gameObject);
                }
            }
        }
        Debug.DrawLine(_base.transform.position, _tip.transform.position, Color.green, 0.1f);
    }


    void LateUpdate()
    {
        // Follow player position
        transform.position = player.transform.position + player.transform.forward * forwardOffset;

        // Face forward in camera direction (flat)
        Vector3 lookDirection = Camera.main.transform.forward;
        //lookDirection.y = 0f;
        lookDirection.Normalize();

        if (lookDirection.sqrMagnitude > 0f)
        {
            // Base look rotation (camera direction)
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);

            // Apply extra Y-axis offset (rotate left/right)
            Quaternion offsetRotation = Quaternion.Euler(0, 90f, -20); // -20° = slightly to the left

            // Combine rotations
            transform.rotation = lookRotation * offsetRotation;
        }

        if (!_isTrailActive)
            return;

        if (_frameCount == (_trailFrameLength * NUM_VERTICES))
            _frameCount = 0;

        _vertices[_frameCount] = _base.transform.position;
        _vertices[_frameCount + 1] = _tip.transform.position;
        _vertices[_frameCount + 2] = _previousTipPosition;
        _vertices[_frameCount + 3] = _base.transform.position;
        _vertices[_frameCount + 4] = _previousTipPosition;
        _vertices[_frameCount + 5] = _tip.transform.position;

        _vertices[_frameCount + 6] = _previousTipPosition;
        _vertices[_frameCount + 7] = _base.transform.position;
        _vertices[_frameCount + 8] = _previousBasePosition;
        _vertices[_frameCount + 9] = _previousTipPosition;
        _vertices[_frameCount + 10] = _previousBasePosition;
        _vertices[_frameCount + 11] = _base.transform.position;

        _triangles[_frameCount] = _frameCount;
        _triangles[_frameCount + 1] = _frameCount + 1;
        _triangles[_frameCount + 2] = _frameCount + 2;
        _triangles[_frameCount + 3] = _frameCount + 3;
        _triangles[_frameCount + 4] = _frameCount + 4;
        _triangles[_frameCount + 5] = _frameCount + 5;
        _triangles[_frameCount + 6] = _frameCount + 6;
        _triangles[_frameCount + 7] = _frameCount + 7;
        _triangles[_frameCount + 8] = _frameCount + 8;
        _triangles[_frameCount + 9] = _frameCount + 9;
        _triangles[_frameCount + 10] = _frameCount + 10;
        _triangles[_frameCount + 11] = _frameCount + 11;

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;

        _previousTipPosition = _tip.transform.position;
        _previousBasePosition = _base.transform.position;
        _frameCount += NUM_VERTICES;
    }

    void SetCountText()
    {
        scoreText.text = "Score: " + count.ToString();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (!_isAttacking) return;
        _triggerEnterTipPosition = _tip.transform.position;
        _triggerEnterBasePosition = _base.transform.position;
    }

    private void OnTriggerExit(Collider collision)
    {
        if (!_isAttacking) return;
        _triggerExitTipPosition = _tip.transform.position;

        //Create a triangle between the tip and base so that we can get the normal
        Vector3 side1 = _triggerExitTipPosition - _triggerEnterTipPosition;
        Vector3 side2 = _triggerExitTipPosition - _triggerEnterBasePosition;

        //Get the point perpendicular to the triangle above which is the normal
        //https://docs.unity3d.com/Manual/ComputingNormalPerpendicularVector.html
        Vector3 normal = Vector3.Cross(side1, side2).normalized;

        //Transform the normal so that it is aligned with the object we are slicing's transform.
        Vector3 transformedNormal = ((Vector3)(collision.gameObject.transform.localToWorldMatrix.transpose * normal)).normalized;

        //Get the enter position relative to the object we're cutting's local transform
        Vector3 transformedStartingPoint = collision.gameObject.transform.InverseTransformPoint(_triggerEnterTipPosition);

        Plane plane = new Plane();

        plane.SetNormalAndPosition(
                transformedNormal,
                transformedStartingPoint);

        var direction = Vector3.Dot(Vector3.up, transformedNormal);

        //Flip the plane so that we always know which side the positive mesh is on
        if (direction < 0)
        {
            plane = plane.flipped;
        }

        GameObject[] slices = Slicer.Slice(plane, collision.gameObject);
        Destroy(collision.gameObject);

        // Apply interaction logic to slices
        foreach (GameObject slice in slices)
        {
            Rigidbody rb = slice.GetComponent<Rigidbody>();
            Collider col = slice.GetComponent<Collider>();
            StartCoroutine(DisableInteractionAfterDelay(slice, rb, col, 5f));
            count += 1;
            SetCountText();
        }

        // Optional: push one half
        if (slices.Length > 1)
        {
            Rigidbody rb = slices[1].GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 force = transformedNormal + Vector3.up * _forceAppliedToCut;
                rb.AddForce(force, ForceMode.Impulse);
            }
        }
    }
    private IEnumerator DisableInteractionAfterDelay(GameObject obj, Rigidbody rb, Collider col, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            Destroy(rb);
        }

        if (col != null)
        {
            col.enabled = false;
        }
    }
    private void AttemptSlice(GameObject target)
    {
        // Avoid double slicing
        if (!_isAttacking || target == null) return;

        _triggerExitTipPosition = _tip.transform.position;

        Vector3 side1 = _triggerExitTipPosition - _triggerEnterTipPosition;
        Vector3 side2 = _triggerExitTipPosition - _triggerEnterBasePosition;
        Vector3 normal = Vector3.Cross(side1, side2).normalized;

        Vector3 transformedNormal = ((Vector3)(target.transform.localToWorldMatrix.transpose * normal)).normalized;
        Vector3 transformedStartingPoint = target.transform.InverseTransformPoint(_triggerEnterTipPosition);

        Plane plane = new Plane();
        plane.SetNormalAndPosition(transformedNormal, transformedStartingPoint);

        if (Vector3.Dot(Vector3.up, transformedNormal) < 0)
            plane = plane.flipped;

        GameObject[] slices = Slicer.Slice(plane, target);
        Destroy(target);

        foreach (GameObject slice in slices)
        {
            Rigidbody rb = slice.GetComponent<Rigidbody>();
            Collider col = slice.GetComponent<Collider>();
            count += 1;
            SetCountText();
        }

        if (slices.Length > 1 && slices[1].TryGetComponent(out Rigidbody pushedRb))
        {
            Vector3 force = transformedNormal + Vector3.up * _forceAppliedToCut;
            pushedRb.AddForce(force, ForceMode.Impulse);
        }
    }
}
