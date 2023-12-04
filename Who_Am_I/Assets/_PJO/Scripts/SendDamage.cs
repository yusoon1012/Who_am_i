using UnityEngine;

public class SendDemage : MonoBehaviour
{
    private ThisData thisData = default;

    private void Start()
    {
        thisData = GetComponentInParent<ThisData>();
        if (thisData == null)
        {
            Debug.LogError("부모오브젝트에 ThisData 컴포넌트가 null입니다.");
            return;
        }
    }

    public void Hit(int _damage)
    {
        thisData.Hit(_damage);
    }
}
