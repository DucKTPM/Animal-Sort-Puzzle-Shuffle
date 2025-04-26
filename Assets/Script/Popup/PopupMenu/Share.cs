using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Share : MonoBehaviour
{
    public void ShareHander()
    {
        new NativeShare()
            .SetText("Chơi game này vui lắm nè! 🎮: https://www.facebook.com/duong.van.uc.163611" )
            //.AddFile("/path/to/image.png") // Nếu bạn muốn chia sẻ ảnh
            .SetSubject("Chia sẻ từ game")
            .SetTitle("Chia sẻ tới bạn bè")
            .Share();

        Debug.Log("Đã gọi share");
    }
}
