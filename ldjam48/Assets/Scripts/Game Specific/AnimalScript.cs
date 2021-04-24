using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalScript : MonoBehaviour
{
    public enum AnimalType {
        Chicken,
        Cow,
        Horse,
        Pig,
        Cat,
        Dog
    }

    [Header("Animal Sprites")]
    public Sprite Chicken;
    public Sprite Cow;
    public Sprite Horse;
    public Sprite Pig;
    public Sprite Cat;
    public Sprite Dog;

    public AnimalType Animal;

    private void Start() {
        SetAnimal();
    }

    private void SetAnimal() {
        switch(Animal) {
            case AnimalType.Chicken:
                GetComponent<SpriteRenderer>().sprite = Chicken;
                break;
            case AnimalType.Cow:
                GetComponent<SpriteRenderer>().sprite = Cow;
                break;
            case AnimalType.Horse:
                GetComponent<SpriteRenderer>().sprite = Horse;
                break;
            case AnimalType.Pig:
                GetComponent<SpriteRenderer>().sprite = Pig;
                break;
            case AnimalType.Cat:
                GetComponent<SpriteRenderer>().sprite = Cat;
                break;
            case AnimalType.Dog:
                GetComponent<SpriteRenderer>().sprite = Dog;
                break;
        }
    }
}
