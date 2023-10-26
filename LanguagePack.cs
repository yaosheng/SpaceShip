using UnityEngine;
using System.Collections;

public class LanguagePack : MonoBehaviour
{
    private string currentLanguage = "";

    public static string teachPlay = "";
    public static string teachShoot = "";
    public static string score = "";
    public static string best = "";

    void Awake( )
    {
        currentLanguage = Application.systemLanguage.ToString( );
        Debug.Log("currentLanguage :" + currentLanguage);
        SwitchLanguage( );
    }

    void SwitchLanguage( )
    {
        if(currentLanguage == "ChineseTraditional") {
            teachPlay = "在正確的時間點擊螢幕，即可飛行到下一個星球";
            teachShoot = "當飛行時，可點擊螢幕來摧毀隕石";
            FBManager.fbContentDescription = "玩膩了要難不難的小遊戲?  那就快來挑戰跳躍星球!";
            score = "分數";
            best = "最高分數";
        }
        else if(currentLanguage == "ChineseSimplified") {
            teachPlay = "在正确的时间点击萤幕，即可飞行到下一个星球";
            teachShoot = "当飞行时，可点击萤幕来摧毁陨石";
            FBManager.fbContentDescription = "玩腻了要难不难的小游戏? 那就快来挑战跳跃星球!";
            score = "分数";
            best = "最高分数";
        }
        else if (currentLanguage == "Vietnamese") {
            teachPlay = "Chạm vào màn hình để du hành đến hành tinh tiếp theo.";
            teachShoot = "Cham vào các thiên thạch để dọn dẹp chúng cho hành trình của bạn";
            FBManager.fbContentDescription = "Đơn giản nhưng đầy thách thức,mau đến chinh phục Orbit Path!";
            score = "Điểm số";
            best = "Điểm cao nhất";
        }
        else if(currentLanguage == "Thai") {
            teachPlay = "แตะหน้าจอให้ทันเวลาที่จะข้ามไปยังดวงดาวถัดไป";
            teachShoot = "กดค้างไว้ ขณะบินไปยังดาวถัดไป เพื่อทำลายสิ่งกีดขวาง";
            FBManager.fbContentDescription = "แตะเข้าสู่ห้วงอวกาศ เกมเล่นไม่ยากแต่ท้าทายฝีมือ แข่งกับเพื่อนๆ";
            score = "คะแนน";
            best = "สุดยอด";
        }
        else if(currentLanguage == "Indonesian") {
            teachPlay = "Sentuh layar di waktu yang tepat jika ingin menyebrangkan roket ke planet selanjutnya";
            teachShoot = "Sentuh layar lagi saat akan menyebrang untuk menembak dan menghancurkan meteor";
            FBManager.fbContentDescription = "Jelajahi luar angkasa! Game simple & menantang cocok dimainkan bersama teman!";
            score = "Skor";
            best = "Rekor";
        }
        else {
            teachPlay = "TAP THE SCREEN AT THE RIGHT TIME TO TRAVEL TO THE NEXT PLANET";
            teachShoot = "PRESS WHILE FLYING TO THE NEXT PLANET TO DESTROY OBSTACLES";
            FBManager.fbContentDescription = "Tap through the space! A simple but challenging game to compete with friends!";
            score = "SCORE";
            best = "BEST";
        }
    }
}
