using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Information : MonoBehaviour
{
    public enum InfoType
    {
        information,
        informationPackage
    }
    public InfoType infoType;

    // Information ohne Object
    public enum Informations
    {
        //BeiMirIsNeAtombombeExplodiert,
        //IchBinAnnektiertWorden
        //HuchEinSchwarzesLoch
    }
    public Informations info;


    // Information mit Gameobject
    public enum Informationpackages
    {
        //GenauDiesesRehLäuftZuDirRüber!
        defectedEntity
    }
    public Informationpackages infopackage;

    public GameObject gameObjectReference;

    public Information(Informations _info)
    {
        infoType = InfoType.information;
        info = _info;
    }

    public Information(Informationpackages _infopackage, GameObject _gameObjectReference)
    {
        infoType = InfoType.informationPackage;
        infopackage = _infopackage;
        gameObjectReference = _gameObjectReference;
    }
}
