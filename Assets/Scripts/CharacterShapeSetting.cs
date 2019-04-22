using UnityEngine;


namespace ICT_Engine
{
    public enum Face
    {
        ES_Asia_Face01,
        ES_Asia_Face02,
        ES_Asia_Face03,
        ES_Asia_Face04,
        ES_Asia_Face05,
        ES_Korea_Face01,
        ES_Korea_Face02,
        ES_Korea_Face03,
        ES_Korea_Face04,
        ES_Korea_Face05,
        Outfit_Helmet
    }

    public enum HandL
    {
        Hand_L,
        Hand_Glove_L,
        Outfit_Glove_L
    }
    public enum HandR
    {
        Hand_R,
        Hand_Glove_R,
        Outfit_Glove_R
    }

    public enum Helmet
    {
        None,
        SafetyHelmet,
    }

    public enum Mask
    {
        None,
        AirRespirator_Mask
    }

    public enum Dress
    {
        Suzuki,
        Suzuki_Fat,
        Suzuki_Slim,
        Outfit,
        Outfit_Up_AirRespirator
    }

    public enum ShoesAll
    {
        Shoes,
        SafetyShoe
    }

    [System.Serializable]
    public class CharacterShapeSetting : MonoBehaviour
    {
        public Face face;
        public HandL handL;
        public HandR handR;
        public Helmet helmet;
        public Mask mask;
        public Dress dress;
        public ShoesAll shoes;

        public GameObject[] gFace;
        public GameObject[] gHandL;
        public GameObject[] gHandR;
        public GameObject[] gHelmet;
        public GameObject[] gMask;
        public GameObject[] gDress;
        public GameObject[] gShoes;

        public bool isChange = false;

        void OnDrawGizmos()
        {
            if (isChange == true)
            {
                ChOri();
                isChange = false;
            }
        }

        private void Awake()
        {
            ChOri();
        }

        public void ChOri()
        {
            CharacterSetChange(face, handL, handR, helmet, mask, dress, shoes);
        }

        public void ChangeCh(Face face, HandL handL, HandR handR, Helmet helmet, Mask mask, Dress dress, ShoesAll shoes)
        {
            CharacterSetChange(face, handL, handR, helmet, mask, dress, shoes);
        }

        public void CharacterSetChange(Face face, HandL handL, HandR handR, Helmet helmet, Mask mask, Dress dress, ShoesAll shoes)
        {
            //Debug.Log("옷 갈아입기 실행");
            string faceName = face.ToString();
            for (int i = 0; i < gFace.Length; i++)
            {
                if (faceName == gFace[i].name) gFace[i].SetActive(true);
                else gFace[i].SetActive(false);
            }

            string handLName = handL.ToString();
            for (int i = 0; i < gHandL.Length; i++)
            {
                if (handLName == gHandL[i].name) gHandL[i].SetActive(true);
                else gHandL[i].SetActive(false);
            }

            string handRName = handR.ToString();
            for (int i = 0; i < gHandR.Length; i++)
            {
                if (handRName == gHandR[i].name) gHandR[i].SetActive(true);
                else gHandR[i].SetActive(false);
            }

            string helmetName = helmet.ToString();
            for (int i = 0; i < gHelmet.Length; i++)
            {
                if (helmetName == gHelmet[i].name) gHelmet[i].SetActive(true);
                else gHelmet[i].SetActive(false);
            }

            string maskName = mask.ToString();
            for (int i = 0; i < gMask.Length; i++)
            {
                if (maskName == gMask[i].name) gMask[i].SetActive(true);
                else gMask[i].SetActive(false);
            }

            string dressName = dress.ToString();
            for (int i = 0; i < gDress.Length; i++)
            {
                if (dressName == gDress[i].name) gDress[i].SetActive(true);
                else gDress[i].SetActive(false);
            }

            string shoesName = shoes.ToString();
            for (int i = 0; i < gShoes.Length; i++)
            {
                if (shoesName == gShoes[i].name) gShoes[i].SetActive(true);
                else gShoes[i].SetActive(false);
            }
        }

    }
}
