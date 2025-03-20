using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoyaMoya : MonoBehaviour
{
    //雲を円形に配置し,中心から距離を鳥ながら徐々にフェードアウトしていきます


    [SerializeField] GameObject Cloud; // 雲のプレファブ
    [SerializeField] int cloudCount = 10; // 生成する雲の数
    [SerializeField] float radius = 2f; // 初期配置の円の半径
    [SerializeField] float fadeDuration = 3f; // フェードアウト時間


    // Start is called before the first frame update
    void Start()
    {
        SpawnCloudsInCircle();
    }


    /// <summary>
    /// 円形に配置
    /// </summary>
    void SpawnCloudsInCircle()
    {
        for (int i = 0; i < cloudCount; i++)
        {
            float angle = i * (360f / cloudCount) * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0) + transform.position;
            GameObject cloudInstance = Instantiate(Cloud, position, Quaternion.identity, transform);
            StartCoroutine(FadeOutAndMove(cloudInstance));
        }
    }


    /// <summary>
    /// フェードアウトしながら外に
    /// </summary>
    /// <param name="cloud"></param>
    /// <returns></returns>
    IEnumerator FadeOutAndMove(GameObject cloud)
    {
        SpriteRenderer sr = cloud.GetComponent<SpriteRenderer>();
        Color startColor = sr.color;
        float timer = 0f;


        //雲一つ一つがランダムに離散するように
        float moveSpeed = Random.Range(0.2f, 1.5f);

        //正規化
        Vector3 moveDirection = (cloud.transform.position - transform.position).normalized;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            cloud.transform.position += moveDirection * moveSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(cloud);
        Destroy(this.gameObject);
    }
    
}
