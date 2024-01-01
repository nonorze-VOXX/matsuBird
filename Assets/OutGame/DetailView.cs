using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailView : MonoBehaviour
{
    public Button backButton;

    public Image bird;

    public TMP_Text name;
    public TMP_Text detail;
    
    // Start is called before the first frame update
    public void SetInfo(int currentType){
        if (currentType == 1)
        {
            name.text = "白眉燕鷗";
            detail.text = "特徵是牠白色的額頭與眉線，從正面看像頭上帶著代表勝利的V字形花紋。<br>築巢位置選擇較保護區裡其他燕鷗謹慎，會尋找比較隱密的處所，通常在草叢內或岩縫裡";
            bird.sprite = Resources.Load<Sprite>("Image/bird/b1");
        }
        else if(currentType ==2)
        {
            name.text = "黑嘴端鳳頭燕鷗";
            detail.text = "稀有夏候鳥、嘴為橘黃色，嘴尖為黑色，是最重要辨識特徵。頭部黑色，具有羽冠，額、胸部白色，背部至尾上覆羽灰色，翼羽灰白色，冬羽頭頂部分白色。";
            bird.sprite = Resources.Load<Sprite>("Image/bird/b2");
        }
        else if (currentType == 3)
        {
            name.text = "紅燕鷗";
            detail.text = "紅燕鷗又稱粉紅燕鷗，因為牠們在繁殖期時，胸前及腹部的與毛會變成淡粉紅色；在繁殖季初期牠們的嘴是黑色的，之後會慢慢變成紅色。 <br>紅燕鷗一窩生2~3個蛋，蛋具有很好的保護色";
            bird.sprite = Resources.Load<Sprite>("Image/bird/b3");
        }
        else if (currentType == 4)
        {
            name.text = "白腰雨燕";
            detail.text = "又名叉尾雨燕，尾羽黑色且分叉，特徵為腰部的白斑 <br>喜成群活動，常是飛得最高的鳥，只有在天氣變差要下雨時，才會飛到較低的高度 ";
            bird.sprite = Resources.Load<Sprite>("Image/bird/b4");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveAllListeners();
    }
}
