using UnityEngine;
using System;
using System.Collections;

public class SpriteAnim : MonoBehaviour
{
    public GameObject AnimatedGameObject;
    public AnimSpriteSet[] AnimationSets;
    private bool AnimActive = false;
    private int Cur_SpriteID;
    private float SecsPerFrame = 0.25f;

    public void PlayAnimation(int ID, float secPerFrame)
    {
        SecsPerFrame = secPerFrame;
        StopCoroutine("AnimateSprite");
        //Add as much ID as necessary. Each is a different animation.
        switch (ID)
        {
            default:
                Cur_SpriteID = 0;
                StartCoroutine("AnimateSprite", ID);
                break;
        }
    }

    public bool Active()
    {
        return AnimActive;
    }

    public void StopAnimation()
    {
        StopCoroutine("AnimateSprite");
    }

    IEnumerator AnimateSprite(int ID)
    {
        switch (ID)
        {
            default:
                AnimActive = true;
                yield return new WaitForSeconds(SecsPerFrame);
                AnimatedGameObject.GetComponent<SpriteRenderer>().sprite
                = AnimationSets[ID].Anim_Sprites[Cur_SpriteID];
                Cur_SpriteID++;
                if (Cur_SpriteID >= AnimationSets[ID].Anim_Sprites.Length)
                {
                    Cur_SpriteID = 0;
                }
                AnimActive = false;
                StartCoroutine("AnimateSprite", ID);
                break;
        }
    }
}

[Serializable]
public class AnimSpriteSet
{
    public string AnimationName;
    public Sprite[] Anim_Sprites;
}