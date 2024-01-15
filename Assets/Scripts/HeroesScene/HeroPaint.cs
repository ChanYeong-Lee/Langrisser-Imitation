using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroPaint : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    private void Start()
    {
        HeroManager.Instance.onChangeGeneral.AddListener(UpdateSprite);
    }

    public void UpdateSprite()
    {
        image.sprite = ResourceManager.Instance.GetGeneralResource(HeroManager.Instance.currentGeneral.GeneralType).generalPaint;
    }
}

