using UnityEngine;

public class SendDamage : MonoBehaviour
{
    private ThisAnimalData thisData = default;

    private void Start()
    {
        thisData = GFunc.SetParentComponent<ThisAnimalData>(this.gameObject);
        if (thisData == null)
        {
            GFunc.SubmitNonFindText<ThisAnimalData>(this.gameObject);
            return;
        }
    }

    public void Hit(int _damage)
    {
        thisData.Hit(_damage);
    }
}