using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DamageUI : MonoBehaviour
{
    public class ActiveText
    {
        public TextMeshProUGUI text;
        public float maxTime;
        public float timer;
        public Vector3 worldPositionStart;
        public Camera cam;

        public void PlaceText()
        {
            float ratio = 1f - (timer / maxTime);
            Vector3 pos = worldPositionStart + new Vector3(ratio, Mathf.Sin(ratio * Mathf.PI), 0);
            pos = cam.WorldToScreenPoint(pos);
            pos.z = 0;
            text.transform.position = pos;
        }
    }

    public static DamageUI instance;
    public TextMeshProUGUI textPrefab;
    public Canvas canvas;
    private Queue<TextMeshProUGUI> textPool = new Queue<TextMeshProUGUI>();
    private List<ActiveText> activeTexts = new List<ActiveText>();
    private Camera cam;
    private const int poolSize = 64;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        for (int i = 0; i < poolSize; i++)
        {
            TextMeshProUGUI t = Instantiate(textPrefab, transform);
            t.gameObject.SetActive(false);
            textPool.Enqueue(t);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < activeTexts.Count; i++)
        {
            ActiveText at = activeTexts[i];
            at.timer -= Time.deltaTime;
            if (at.timer <= 0f)
            {
                at.text.gameObject.SetActive(false);
                textPool.Enqueue(at.text);
                activeTexts.RemoveAt(i);
                i--;
            }
            else
            {
                Color color = at.text.color;
                color.a = at.timer / at.maxTime;
                at.text.color = color;
                at.PlaceText();
            }
        }
    }

    public void NewDamage(int amount, Vector3 worldPos)
    {
        TextMeshProUGUI t = textPool.Dequeue();
        t.text = amount.ToString();
        t.gameObject.SetActive(true);

        ActiveText at = new ActiveText();
        at.maxTime = 1;
        at.timer = at.maxTime;
        at.text = t;
        at.worldPositionStart = worldPos + Vector3.up;
        at.cam = cam;
        at.PlaceText();
        activeTexts.Add(at);
    }
}
