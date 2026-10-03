using UnityEngine;

public class PlayerVisualGenerator : MonoBehaviour
{
    [SerializeField] private Material bodyMaterial;
    [SerializeField] private Material headMaterial;

    private void Start()
    {
        CreateBody();
        CreateHead();
        CreateArms();
        CreateLegs();
    }

    private void CreateBody()
    {
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        body.name = "Body";
        body.transform.SetParent(transform);

        body.transform.localPosition = new Vector3(0f, 1.2f, 0f);
        body.transform.localRotation = Quaternion.identity;
        body.transform.localScale = new Vector3(0.7f, 1f, 0.5f);

        ApplyMaterial(body, bodyMaterial);
    }

    private void CreateHead()
    {
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        head.name = "Head";
        head.transform.SetParent(transform);

        head.transform.localPosition = new Vector3(0f, 2.5f, 0f);
        head.transform.localRotation = Quaternion.identity;
        head.transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);

        ApplyMaterial(head, headMaterial);
    }

    private void CreateArms()
    {
        CreateLimb(
            "LeftArm",
            new Vector3(-0.55f, 1.25f, 0f),
            new Vector3(0.25f, 0.8f, 0.25f)
        );

        CreateLimb(
            "RightArm",
            new Vector3(0.55f, 1.25f, 0f),
            new Vector3(0.25f, 0.8f, 0.25f)
        );
    }

    private void CreateLegs()
    {
        CreateLimb(
            "LeftLeg",
            new Vector3(-0.25f, 0.35f, 0f),
            new Vector3(0.3f, 0.8f, 0.3f)
        );

        CreateLimb(
            "RightLeg",
            new Vector3(0.25f, 0.35f, 0f),
            new Vector3(0.3f, 0.8f, 0.3f)
        );
    }

    private void CreateLimb(string limbName, Vector3 position, Vector3 scale)
    {
        GameObject limb = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        limb.name = limbName;
        limb.transform.SetParent(transform);

        limb.transform.localPosition = position;
        limb.transform.localRotation = Quaternion.identity;
        limb.transform.localScale = scale;

        ApplyMaterial(limb, bodyMaterial);
    }

    private void ApplyMaterial(GameObject objectToChange, Material material)
    {
        if (material == null)
        {
            return;
        }

        Renderer renderer = objectToChange.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material = material;
        }
    }
}