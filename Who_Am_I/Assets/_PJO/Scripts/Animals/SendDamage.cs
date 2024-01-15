using UnityEngine;

public class SendDamage : MonoBehaviour
{
    private Animal thisData = default;

    private void Start()
    {
        thisData = GFunc.SetParentComponent<Animal>(this.gameObject);
        if (thisData == null)
        {
            GFunc.SubmitNonFindText<Animal>(this.gameObject);
            return;
        }
    }

    public void Hit(int _damage) // <Solbin> 데미지를 받는 메소드
    {
        thisData.Hit(_damage);
    }
}