using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;
using Unity.VisualScripting;

public class test : MonoBehaviour {
    public GameObject Reference1;
    public GameObject Reference2;

    public GameObject[] arr;

    public void Start() {
        GameObject tempObj;
        ParticleSystem tempPS;
        FieldInfo[] tempFields;
        PropertyInfo[] tempProperties;

        arr = new GameObject[1000];
        for (int i = 0; i < 1000; i++) {
            tempObj = new GameObject("temp");
            tempObj.AddComponent<ParticleSystem>();
            arr[i] = tempObj;
        }

        tempFields = Reference1.GetComponent<ParticleSystem>().GetType().GetFields();
        tempProperties = Reference1.GetComponent<ParticleSystem>().GetType().GetProperties();
        foreach (GameObject obj in arr) {
            tempPS = obj.GetComponent<ParticleSystem>();
            foreach (FieldInfo fi in tempFields) {
                if (!fi.IsStatic) {
                    fi.SetValue(tempPS, fi.GetValue(Reference1));
                }
            }
            foreach (PropertyInfo pi in tempProperties) {
                if (pi.CanWrite && pi.Name != "name") {
                        pi.SetValue(tempPS, pi.GetValue(Reference1));
                }
            }
        }

        tempFields = Reference2.GetComponent<ParticleSystem>().GetType().GetFields();
        tempProperties = Reference2.GetComponent<ParticleSystem>().GetType().GetProperties();
        foreach (GameObject obj in arr) {
            tempPS = obj.GetComponent<ParticleSystem>();
            foreach (FieldInfo fi in tempFields) {
                if (!fi.IsStatic) {
                    fi.SetValue(tempPS, fi.GetValue(Reference2));
                }
            }
            /*
            foreach (PropertyInfo pi in tempProperties) {
                if (pi.CanWrite || pi.Name != "name") {
                    pi.SetValue(tempPS, pi.GetValue(Reference2));
                }
            }
            */
        }

        tempFields = Reference1.GetComponent<ParticleSystem>().GetType().GetFields();
        tempProperties = Reference1.GetComponent<ParticleSystem>().GetType().GetProperties();
        foreach (GameObject obj in arr) {
            tempPS = obj.GetComponent<ParticleSystem>();
            foreach (FieldInfo fi in tempFields) {
                if (!fi.IsStatic) {
                    fi.SetValue(tempPS, fi.GetValue(Reference1));
                }
            }
            /*
            foreach (PropertyInfo pi in tempProperties) {
                if (pi.CanWrite || pi.Name != "name") {
                    pi.SetValue(tempPS, pi.GetValue(Reference1));
                }
            }
            */
        }

        tempFields = Reference2.GetComponent<ParticleSystem>().GetType().GetFields();
        tempProperties = Reference2.GetComponent<ParticleSystem>().GetType().GetProperties();
        foreach (GameObject obj in arr) {
            tempPS = obj.GetComponent<ParticleSystem>();
            foreach (FieldInfo fi in tempFields) {
                if (!fi.IsStatic) {
                    fi.SetValue(tempPS, fi.GetValue(Reference2));
                }
            }
            /*
            foreach (PropertyInfo pi in tempProperties) {
                if (pi.CanWrite || pi.Name != "name") {
                    pi.SetValue(tempPS, pi.GetValue(Reference2));
                }
            }
            */
        }

        tempFields = Reference1.GetComponent<ParticleSystem>().GetType().GetFields();
        tempProperties = Reference1.GetComponent<ParticleSystem>().GetType().GetProperties();
        foreach (GameObject obj in arr) {
            tempPS = obj.GetComponent<ParticleSystem>();
            foreach (FieldInfo fi in tempFields) {
                if (!fi.IsStatic) {
                    fi.SetValue(tempPS, fi.GetValue(Reference1));
                }
            }
            /*
            foreach (PropertyInfo pi in tempProperties) {
                if (pi.CanWrite || pi.Name != "name") {
                    pi.SetValue(tempPS, pi.GetValue(Reference1));
                }
            }
            */
        }
    }
}
