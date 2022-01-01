using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptUtils.Character
{
    public class SimpleArrowKeyMover : MonoBehaviour
    {
        /// <summary>
        /// Wether character can move in any direction
        /// Must be implemented for ANDROID!
        /// </summary>
        public bool canMove = true;
        /// <summary>
        /// Wether character can move up
        /// </summary>
        public bool canMoveUp = false;
        /// <summary>
        /// Wether character can move down
        /// </summary>
        public bool canMoveDown = false;
        /// <summary>
        /// Wether character can move right
        /// </summary>
        public bool canMoveRight = false;
        /// <summary>
        /// Wether character can move down
        /// </summary>
        public bool canMoveLeft = false;
        public float horizontalSpeed;
        public float verticalSpeed;

        public float minX;
        public float maxX;
        public float minY;
        public float maxY;

        private bool goLeft = false;
        private bool goRight = false;
        private bool goUp = false;
        private bool goDown = false;
        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || goLeft)
                moveLeft();
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || goRight)
                moveRight();
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || goUp)
                moveUp();
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || goDown)
                moveDown();
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                goDown = false;
                goUp = false;
                goRight = false;
                goLeft = false;
            }
        }
        public void moveLeft()
        {
            if (!canMove || !canMoveLeft)
                return;
            float horizontalChange = horizontalSpeed * Time.deltaTime;
            Vector3 cPosition = gameObject.transform.position;
            if (cPosition.x - horizontalChange >= minX)
                cPosition.x -= horizontalChange;
            gameObject.transform.position = cPosition;
        }
        public void moveRight()
        {
            if (!canMove || !canMoveRight)
                return;
            float horizontalChange = horizontalSpeed * Time.deltaTime;
            Vector3 cPosition = gameObject.transform.position;
            if (cPosition.x + horizontalChange <= maxX)
                cPosition.x += horizontalChange;
            gameObject.transform.position = cPosition;
        }
        public void moveUp()
        {
            if (!canMove || !canMoveUp)
                return;
            float verticalChange = verticalSpeed * Time.deltaTime;
            Vector3 cPosition = gameObject.transform.position;
            if (cPosition.y + verticalChange <= maxY)
                cPosition.y += verticalChange;
            gameObject.transform.position = cPosition;
        }
        public void moveDown()
        {
            if (!canMove || !canMoveDown)
                return;
            float verticalChange = verticalSpeed * Time.deltaTime;
            Vector3 cPosition = gameObject.transform.position;
            if (cPosition.y - verticalChange >= minY)
                cPosition.y -= verticalChange;
            gameObject.transform.position = cPosition;
        }
    }
}