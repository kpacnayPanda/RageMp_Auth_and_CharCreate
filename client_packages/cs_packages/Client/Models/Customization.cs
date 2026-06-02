using System.Collections.Generic;

public class Customization
{
    private static Dictionary<string, int[,]> maleClothes = new Dictionary<string, int[,]>()
    {
        {"torso", new [,]
            {
                {22, 0},
                {16, 0},
                {3, 14}
            }
        },
        {"legs", new [,]
            {
                {43},
                {23},
                {13}
            }
        },
        {"shoes", new [,]
        {
            {1},
            {6},
            {-1}
        }}
    };
    
    private static Dictionary<string, int[,]> femaleClothes = new Dictionary<string, int[,]>()
    {
        {"torso", new [,]
            {
                {49, 0},
                {280, 0},
                {14, 0}
            }
        },
        {"legs", new [,]
            {
                {3},
                {41},
                {14}
            }
        },
        {"shoes", new [,]
            {
                {3},
                {5},
                {-1}
            }
        }
    };
    
    public static void ResetLocalPlayerCustomization(string gender = "male")
    {
        RAGE.Elements.Player.LocalPlayer.SetHeadBlendData(21, 0, 0, 21, 0, 0, 0.5f, 0.5f, 0, true);
        RAGE.Elements.Player.LocalPlayer.SetEyeColor(0);
        RAGE.Elements.Player.LocalPlayer.SetHairColor(0, 0);

        for (int i = 0; i <= 19; i++)
        {
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(i, 0);
        }

        ResetLocalPlayerClothes(gender);
    }

    private static void ResetLocalPlayerClothes(string gender = "male")
    {
        RAGE.Elements.Player.LocalPlayer.SetComponentVariation(11, GetStarterTopClothesById("torso", 0, gender).Item1, 0,0); // tops
        RAGE.Elements.Player.LocalPlayer.SetComponentVariation(3, GetStarterTopClothesById("torso", 0, gender).Item2, 0,0); // torso
        RAGE.Elements.Player.LocalPlayer.SetComponentVariation(8, 15, 0,0); // undershirt
        
        RAGE.Elements.Player.LocalPlayer.SetComponentVariation(4, GetStarterClothesById("legs", 0, gender), 0,0); // legs
        RAGE.Elements.Player.LocalPlayer.SetComponentVariation(6, GetStarterClothesById("shoes", 0, gender), 0,0); // shoes
    }

    public static void SetLocalPlayerCustomization(dynamic customizationInfo, string gender)
    {
        byte first = (byte)customizationInfo.firstParent;
        byte second = (byte)customizationInfo.secondParent;
        float shapeMix = (float)customizationInfo.shapeMix;
        float skinMix = (float)customizationInfo.skinMix;

        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(0, (float)customizationInfo.noseWidth);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(1, (float)customizationInfo.noseHeight);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(2, (float)customizationInfo.noseLength);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(3, (float)customizationInfo.noseBridge);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(4, (float)customizationInfo.noseTip);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(5, (float)customizationInfo.noseBridgeShift);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(6, (float)customizationInfo.browHeight);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(7, (float)customizationInfo.browWidth);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(8, (float)customizationInfo.checkBoneHeight);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(9, (float)customizationInfo.checkBoneWidth);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(10, (float)customizationInfo.checkWidth);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(11, (float)customizationInfo.eyes);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(12, (float)customizationInfo.lips);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(13, (float)customizationInfo.jawWidth);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(14, (float)customizationInfo.jawHeight);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(15, (float)customizationInfo.chinLength);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(16, (float)customizationInfo.chinPosition);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(17, (float)customizationInfo.chinWidth);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(18, (float)customizationInfo.chinShape);
        RAGE.Elements.Player.LocalPlayer.SetFaceFeature(19, (float)customizationInfo.neckWidth);
        
        RAGE.Elements.Player.LocalPlayer.SetEyeColor((int)customizationInfo.eyeColor);
        RAGE.Elements.Player.LocalPlayer.SetHairColor((int)customizationInfo.firstHairColor, (int)customizationInfo.secondHairColor);
        RAGE.Elements.Player.LocalPlayer.SetComponentVariation(2, (int)customizationInfo.hairStyle, 0,0); // haircut
        
        RAGE.Elements.Player.LocalPlayer.SetComponentVariation(11, GetStarterTopClothesById("torso", (int)customizationInfo.top, gender).Item1, 0,0); // tops
        RAGE.Elements.Player.LocalPlayer.SetComponentVariation(3, GetStarterTopClothesById("torso", (int)customizationInfo.top, gender).Item2, 0,0); // torso
        RAGE.Elements.Player.LocalPlayer.SetComponentVariation(8, 15, 0,0); // undershirt
        RAGE.Elements.Player.LocalPlayer.SetComponentVariation(4, GetStarterClothesById("legs", (int)customizationInfo.legs, gender), 0,0); // legs
        RAGE.Elements.Player.LocalPlayer.SetComponentVariation(6, GetStarterClothesById("shoes", (int)customizationInfo.shoes, gender), 0,0); // shoes
        
        RAGE.Elements.Player.LocalPlayer.SetHeadBlendData(first, second, 0, first, second, 0, shapeMix, skinMix, 0, true);
    }

    private static (int, int) GetStarterTopClothesById(string component, int id, string gender)
    {
        return gender == "male"
            ? (maleClothes[component][id, 0], maleClothes[component][id, 1])
            : (femaleClothes[component][id, 0], femaleClothes[component][id, 1]);
    }

    private static int GetStarterClothesById(string component, int id, string gender)
    {
        return gender == "male" ? maleClothes[component][id, 0] : femaleClothes[component][id, 0];
    }
    
    
}