using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Define;

public class BubbleDragonManager : MonoBehaviour
{
    static BubbleDragonManager _instance;

    public static BubbleDragonManager Instance {  get { return _instance; } }

    Dictionary<int , BubbleDragon> _dragons = new Dictionary<int, BubbleDragon> ();


    [SerializeField]
    GameObject myDragonPrefab; 
    [SerializeField]
    GameObject otherDragonPrefab;

    // Start is called before the first frame update

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }
  
    public void Instantiate_MyDragon(PlayerInfo playerInfo)
    {
        GameObject myDragon = Instantiate(myDragonPrefab, new Vector2(playerInfo.posX, playerInfo.posY), Quaternion.identity);
      
        myDragon.GetComponent<MyBubbleDragon>()._id = playerInfo.id;
        myDragon.GetComponent<MyBubbleDragon>().speed = playerInfo.speedPerSec;

        

        GameManager.Instance._myDragon = myDragon.GetComponent<MyBubbleDragon>();

        _dragons.Add(playerInfo.id, GameManager.Instance._myDragon);   
    } 
    
    public void Instantiate_OtherDragon(PlayerInfo playerInfo)
    {
        GameObject otherDragon = Instantiate(otherDragonPrefab, new Vector2(playerInfo.posX, playerInfo.posY), Quaternion.identity);
       
        otherDragon.GetComponent<OtherBubbleDragon>()._id = playerInfo.id;
        otherDragon.GetComponent<OtherBubbleDragon>().speed = playerInfo.speedPerSec;



        


        _dragons.Add(playerInfo.id, otherDragon.GetComponent<OtherBubbleDragon>());

    }

    public void Instantiate_OtherDragon(List<PlayerInfo> infoList )
    {
        for(int i = 0; i < infoList.Count; i++)
        {
            Instantiate_OtherDragon(infoList[i]);
        }
    }

    public void SetMoveDirByid(int id, MoveDir dir)
    {
       
        _dragons.TryGetValue(id, out BubbleDragon dragon);

        dragon.SetDir(dir);
    }

    public void KickById(int id)
    {
        _dragons.TryGetValue(id, out BubbleDragon dragon);

        Destroy(dragon.gameObject);
        _dragons.Remove(id);
    }
    public void JumpById(int id, int force)
    {
        _dragons.TryGetValue(id, out BubbleDragon dragon);
        dragon.Jump(force);

    }

    public void MakeBubbleById(int id , float posX, float posY, bool isRight)
    {
        _dragons.TryGetValue(id, out BubbleDragon dragon);
        dragon.MakeBubble(id, posX, posY, isRight);
    }

    public void MakeBubble360ById(int id)
    {
        _dragons.TryGetValue(id, out BubbleDragon dragon);
        dragon.MakeBubble360(id);
    }

    public void HealAllDragonExceptMe(int id)
    {
        foreach(BubbleDragon dragon in _dragons.Values)
        {
            //if(dragon._id == id)
            //{
            //    continue;
            //}
            dragon.OnHeal();
        }
    }

    public void HealDragon(int id)
    {
        _dragons[id].OnHeal();
    }
    public void DestroyDragon(int id)
    {
        _dragons.TryGetValue(id, out BubbleDragon dragon);
        Destroy(dragon.gameObject);

        _dragons.Remove(id);
    }

    public string GetNickNameById(int id)
    {
        _dragons.TryGetValue(id, out BubbleDragon dragon);
        return dragon.nickName;
    }

}
