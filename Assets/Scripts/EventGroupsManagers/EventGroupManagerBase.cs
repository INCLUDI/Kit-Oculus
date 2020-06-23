﻿using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static DataModel;

public abstract class EventGroupManagerBase : ScriptableObject
{
    protected Vector3 _initialScale;

    public List<string> Correct { get => ActivityManager.instance.CurrentEvent.instructions.correct; }
    public List<string> Wrong { get => ActivityManager.instance.CurrentEvent.instructions.wrong; }
    
    public virtual void Ready() { }

    public virtual List<SceneObj> randomizeObjects(List<SceneObj> objs, int objectsToSpawn)
    {
        System.Random rnd = new System.Random();
        return objs.OrderBy(c => rnd.Next())
            .Take(objectsToSpawn)
            .ToList();
    }


    public virtual void checkCorrectAction(GameObject interactable) { }

    public virtual void checkCorrectAction(GameObject target, GameObject interactable = null) { }

    public virtual void checkCorrectAction(string answer) { }

    public virtual string selectRequest(List<string> instructions)
    {
        return selectRandomInstruction(instructions);
    }

    public virtual string selectCorrect(List<string> instructions, string param = null)
    {
        return selectRandomInstruction(instructions);
    }

    public virtual string selectWrong(List<string> instructions, string param = null)
    {
        return selectRandomInstruction(instructions);
    }

    private string selectRandomInstruction(List<string> instructions)
    {
        if (instructions != null && instructions.Count != 0)
        {
            System.Random rnd = new System.Random();
            return instructions?[rnd.Next(0, instructions.Count)];
        }
        else return null;
    }

    protected virtual void StopVisualFeedback(GameObject feedback, UnityAction call = null)
    {
        feedback.transform.DOScale(new Vector3(0, 0, 0), 0.5f).OnComplete(() =>
        {
            feedback.SetActive(false);
            feedback.transform.localScale = _initialScale;
            call?.Invoke();
        });
    }

    protected virtual void ParameterObjects()
    {
        foreach (string toRemove in ActivityManager.instance.Parameters.objsToDeactivate)
        {
            GameObject objToRemove = GameObject.Find(toRemove);
            objToRemove.transform.DOScale(new Vector3(0, 0, 0), 1).OnComplete(() => Destroy(objToRemove));
        };
        foreach (SceneObj obj in ActivityManager.instance.Parameters.objsToActivate)
        {
            ActivityManager.instance.InstantiateObj(obj);
        }
    }

    protected virtual void SetFinalPosition(GameObject interactable)
    {
        if (ActivityManager.instance.Parameters.finalTransform.position != null)
        {
            Vector3 finalPosition = new Vector3(
                ActivityManager.instance.Parameters.finalTransform.position.x,
                ActivityManager.instance.Parameters.finalTransform.position.y,
                ActivityManager.instance.Parameters.finalTransform.position.z);
            interactable.transform.DOMove(finalPosition, 1);
        }
    }

    protected virtual void SetFinalRotation(GameObject interactable)
    {
        if (ActivityManager.instance.Parameters.finalTransform.rotation != null)
        {
            Vector3 finalRotation = new Vector3(
                ActivityManager.instance.Parameters.finalTransform.rotation.x,
                ActivityManager.instance.Parameters.finalTransform.rotation.y,
                ActivityManager.instance.Parameters.finalTransform.rotation.z);
            interactable.transform.DORotate(finalRotation, 1);
        }
    }
}
