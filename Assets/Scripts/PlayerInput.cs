using System;
using UnityEngine;
using UnityEngine.SceneManagement;
internal class PlayerInput : MonoBehaviour
{
    public bool RotateLeft { get; private set; }
    public bool RotateRight { get; private set; }
    public bool Thrust { get; private set; }
    public bool DisableCollision { get; private set; }

    void Update()
    {
        Thrust = Input.GetKey(KeyCode.Space);
        RotateLeft = Input.GetKey(KeyCode.A);
        RotateRight = Input.GetKey(KeyCode.D);
        if (Input.GetKeyDown(KeyCode.C))
            DisableCollision = !DisableCollision;
        if (Input.GetKeyUp(KeyCode.L))
            GetComponent<CollisionHandler>().LoadNextLevel();
    }
}