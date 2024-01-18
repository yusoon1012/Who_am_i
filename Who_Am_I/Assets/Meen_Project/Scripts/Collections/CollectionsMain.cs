using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionsMain
{
    // 컬렉션 이름
    public string name = default;
    // 컬렉션 정보
    public string info = default;
    // 컬렉션 획득 장소 힌트
    public string hint = default;
    // 컬렉션 이미지 아이콘 번호
    public int imageNum = default;
    // 컬렉션 타입 구분
    public int collectionType = default;

    public virtual void Init()
    {
        /* Empty */
    }     // Init()
}
