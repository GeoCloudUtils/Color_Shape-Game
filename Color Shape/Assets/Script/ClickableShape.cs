using EduUtils.Events;
using ScriptUtils;
using UnityEngine;
using UnityEngine.Events;

public enum ShapeType
{
    SQUARE = 1,
    RECTANGLE = 2,
    TRIANGLE = 3,
    CIRCLE = 4,
    STAR
}
public class ClickableShape : ObjectSequence
{
    public ShapeType shapeType = ShapeType.SQUARE;
    public event UnityAction<ClickableShape, string, ShapeType> ShapeClick;
    public bool clickable = true;
    public void AddClickEvent()
    {
        CurrentChild.AddComponent<MouseEventSystem>().MouseEvent += OnShapeClick;
    }

    public void LateUpdate()
    {
        shapeType = (ShapeType)CurrentChildIndex;
    }
    public void SetLayerPriority()
    {
        CurrentChild.GetComponent<SpriteRenderer>().sortingOrder += 1;
    }

    private void OnShapeClick(GameObject target, MouseEventType type)
    {
        if (type == MouseEventType.CLICK && clickable)
        {
            ShapeClick?.Invoke(this, CurrentChild.name, shapeType);
        }
    }
}
