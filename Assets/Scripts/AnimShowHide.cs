using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;
using UnityEngine.UI;

namespace ICT_Engine
{
    public class AnimShowHide : MonoBehaviour
    {
        public GameObject viewPort;
        public GameObject content;
        public Text bubbleText;

        Del delUpdate;
        Del delEnd;
        // 이전 스케일 조정시 사용했던 변수.
        //float scX;

        Image baseImage;
        // 새롭게 투명 반투명으로 변화되는 것을 하기 위해 새로 지정하는 변수.
        float alphaVal;
        // 최초 색을 보존하기 위해 지정하는 컬러벡터.
        Color oriColor;

        // 알파값을 변화시켜주기 위해 필요한 변수.
        float alphaChanged;

        public float scrollPerSec = 50f;

        private void Awake()
        {
            //scX = transform.localScale.x;
            // 원본 이미지, 컬러와 알파를 각각 저장해놓는다.
            baseImage = GetComponent<Image>();
            oriColor = baseImage.color;

        }
        void OnEnable()
        {
            // 텍스트 박스가 잘 보일때는 알파값은 1이다.
            alphaVal = 1f;
            // 최초의 알파는 투명해야 되니 값을 0으로 지정한다.
            alphaChanged = 0f;
            // 처음에 대화창이 나올때는 투명에서 시작한다.
            baseImage.color = ChangeAlpha(alphaChanged, baseImage.color);
            // 글씨 투명도 역시 똑같이 변화하게 해준다.
            bubbleText.color = ChangeAlpha(alphaChanged, bubbleText.color);
            // 알파값을 변화시켜주는 함수를 delegate 등록한다.
            delUpdate += ShowTextBox;

            sLength = 0f;

            isRolling = false;
            spendTime = 0f;
        }

        Color ChangeAlpha(float a, Color baseColor)
        {
            Color c = new Color();
            c = new Color(baseColor.r, baseColor.g, baseColor.b, a);
            return c;
        }

        //외부에서 차후에 실행할 함수를 등록한다//
        public void InDel(Del inD)
        {
            delEnd = inD;
        }

        AudioSource aSource;

        // 음성의 총길이를 측정저장하기 위해 필요한 변수.
        float sLength;
        bool isPlayAudio = false;
        float actionNum;
        public void GetSoundA(AudioSource source, float sl, bool isPlay, float aNum)
        {
            actionNum = aNum;
            aSource = source;
            isPlayAudio = isPlay;
            //음성길이가 0보다 크면 음성파일의 재생시간을 말풍선이 보이는 시간으로 설정하고//
            if (sl > 0)
                sLength = sl;
            //음성길이가 0이며 기존에 세팅되어있는 디폴트 값을 이용한다//
            else
                sLength = 0;
        }

        //텍스트 박스를 보이게 하는 함수.
        void ShowTextBox()
        {
            //Debug.Log("=====ShowTextBox");
            //Debug.Log("박스보이기 시작 ::: 박스 알파: "+ baseImage.color.a);
            // 2f를 곱한 것은 두배 빠르게 알파의 변화를 얻기 위해서.
            // 결국 0.5초면 1의 값에 도달한다.
            // 최초값인 alphaval을 곱하는 것은.
            // (Time.deltaTime * 2f)의 값이 1이 되었을때 alphaVal과 같은 값이 되도록 하기 위해서
            alphaChanged += (Time.deltaTime * 2f) * alphaVal;

            if (alphaChanged < alphaVal)
            {
                baseImage.color = ChangeAlpha(alphaChanged, baseImage.color);
                bubbleText.color = ChangeAlpha(alphaChanged, bubbleText.color);
            }
            else
            {
                // 텍스트 박스의 알파 애니를 끝낸다.
                // (원래 값을 넣는다)
                alphaChanged = alphaVal;
                baseImage.color = ChangeAlpha(1f, baseImage.color);
                bubbleText.color = ChangeAlpha(1f, bubbleText.color);

                // 텍스트의 롤링을 관장하는 함수를 실행한다.
                SetTextPosition();
                // 플레이어 일경우 오디오를 실행한다.
                if (isPlayAudio == true)
                    aSource.Play();
                
                // delegate로 들어가 있던 ShowTextBox를 제거한다.
                delUpdate -= ShowTextBox;
            }
        }

        // 기본 텍스트를 보여주는 시간을 세팅한다.
        // 하나의 말풍선이 끝나면 항상 기본 값으로 돌려주는 작업이 꼭 들어가야 한다.
        public float textShowTime = 2f;
        // Content의 새로운 Pos Y 값을 지정하기 위해 생성한 변수.
        // 롤링이 필요없으면 Pos Y의 값은 0이다.
        // 초기화 값을 0으로 하면 코딩양을 줄일 수 있다.
        // Content의 길이가 더 짧거나 또는 같을 경우(스크롤이 필요없는 경우)
        // Content의 Pos Y를 0으로 한다.
        float newPosY = 0;
        // 길이가 더 짧으면 롤링을 하지 않을테니 기본값은 false로 한다.
        bool isRolling = false;

        // 아래 네개의 변수는ViewPort와 content의 위치와 크기를 관장하는 트랜스폼과 값을 저장하기 위한 것이다.
        RectTransform viewRectT;
        RectTransform conRectT;
        float viewHeight;
        float contHeight;

        // Content의 Pos Y 값은 변경되니 미리 Rect 값을 받아놓는다.
        // Pos Y만 따로 떼어내서 변경이 불가능한 구조라서 Rect 전체를 받아서.
        // 변경하여야 한다.
        Rect cr;
        Vector3 crp;

        void SetTextPosition()
        {
            //Debug.Log("=====SetTextPosition");

            //1. 사운드가 있는지 먼저 확인한다.
            //2. 사운드가 있다면...
            //2-1. (true)(사운드있음) 텍스트가 롤링이 필요한가 확인한다.
            //2-1-1. (true) 롤링 텍스트의 총 롤링 시간과 음성의 총 시간을 비교하여 
            //    더 긴쪽의 시간을 취하여 총 텍스트박스 유지 시간으로 한다.(앞뒤 페이드인아웃은 예외)
            //2-1-2. (false) 롤링이 필요없으니 음성의 길이(시간)을 텍스트 박스 유지 시간으로 한다.
            //    만약 최소 시간보다 음성의 길이(시간을)가 적다면 최소 시간을 유지 시간으로 한다.
            //2-2. (false)(사운드 없음) 텍스트가 취하는 시간(총 롤링 시간-롤링을 하지 않는 다면)이
            //    최소시간보다 짧으면 최소시간을 텍스트박스 유지시간으로 한다.


            // ViewPort와 Content의 Height를 비교하여 롤링여부와 그와 관련하여.
            // Content의 PosY를 어떻게 재정의 할지를 결정한다. 
            viewRectT = viewPort.GetComponent<RectTransform>();
            conRectT = content.GetComponent<RectTransform>();

            // 아래 두 변수는 ViewPort와 Content의 height 값을 비교하기 위해 미리 선언하고.
            // 가져와야 될 값으로 초기화 한 변수이다.
            viewHeight = viewRectT.rect.height;
            contHeight = conRectT.rect.height;

            // Content의 Pos Y 값은 변경되니 미리 Rect 값을 받아놓는다.
            // Pos Y만 따로 떼어내서 변경이 불가능한 구조라서 Rect 전체를 받아서.
            // 변경하여야 한다.
            cr = conRectT.rect;
            crp = conRectT.transform.localPosition;

            newPosY = 0;
            //Debug.Log("기본은 "+ newPosY+"이다.");
            if (viewHeight < contHeight)
            {
                // Content 길이가 더 길경우
                // Content의 Pos Y의 값을 조정한다.
                Debug.Log("cr.height: " + cr.height);
                newPosY = (contHeight / -2)+ viewHeight/2;
                Debug.Log("newPosY: " + newPosY);

                // 길이가 더 기므로 isRolling 값을 true로 변경한다.
                // 아래 코드에서 이 변수값을 활용해서 롤링을 할지 말지를 결정하는 코드가 작성된다.
                isRolling = true;

                //Debug.Log("길이 :: " + cr.height + "::: 더 길다.::: " + newPosY);
            }
            else
            {
                isRolling = false;
                //Debug.Log("길이 :: " + cr.height + "::: 길지 않다.::: " + newPosY);
            }

            conRectT.transform.localPosition = new Vector3(crp.x, newPosY,crp.z);


            //1. 사운드가 있는지 먼저 확인한다.
            //2. 사운드가 있다면...
            // 0 보다 크면 사운드가 있는다는 의미이다.
            // 보여지는 기본 시간은 2초로 한다.
            textShowTime = 2f;
            if (sLength > 0)
            {
                //Debug.Log("사운드가 있다 __ 길이는::: "+sLength);
                // 롤링을 하게 된다면 롤링하면서 소요되는 시간을 미리 계산하여 본다.
                if (isRolling == true)
                {
                    textShowTime = GetRollingTime(contHeight);
                }

                //계산된 시간이 오히려 짧다면 사운드의 시간을 넣어준다.
                if (textShowTime < sLength)
                    textShowTime = sLength;
            }
            //2-2. (false)(사운드 없음) 텍스트가 취하는 시간(총 롤링 시간-롤링을 하지 않는 다면)이
            //    최소시간보다 짧으면 최소시간을 텍스트박스 유지시간으로 한다.
            else
            {
                //Debug.Log("사운드가 없다.");
                // 롤링을 할 정도의 문장 길이가 안되면 2초로 제한한다.

                //그리고 롤링을 한다면 총 롤링 시간을 정한다음에 텍스트 박스를 꺼주는 쪽으로 코드를 작성한다.
                if (isRolling == true)
                {
                    // 텍스트의 양이 많아서 롤링을 하게 된다면
                    // Content의 길이를 기준으로 70을 1초동안 움직이도록 설정을 한다.
                    // 그렇게 하여 그 값을 총 움직이는 시간 또는 보여주는 시간으로 설정하고
                    // 미리 시간을 생성하여준다.
                    // 그래서 이 것을 총나타나는 시간이라 칭하고 textShowTime에 그 값을 지정하여준다.
                    // 이 부분은 한번에 계산하고 끝나는 것이니
                    // 함수를 따로 만들어서 계산하도록 하여준다.
                    // 총 시간을 계산하여야 되니 컨텐트의 하이트 값을 시간 계산을 위해 함수에 인자값으로 넘긴다.
                    textShowTime = GetRollingTime(contHeight);
                }
            }

            // 텍스트 포지션과 보여지는 총시간을 모두 설정하였으면
            // 실제 텍스트 움직이는 애니와 텍스트박스 유지시간을 관장하는 함수를 실행한다.
            //Debug.Log("MovingText ___ Action Number ::: " + actionNum);
            if (isAddMovingTextM == false)
            {
                isAddMovingTextM = true;

                delUpdate += MovingText;
            }
            else
            {
                //Debug.Log("이미 메소드 데리게이트로 넘겼다.");
            }

            //if (spendTime < sLength)
            //{
            //    spendTime += Time.deltaTime;
            //}
            //else
            //{
            //    spendTime = 0;
            //    delUpdate += HideTextBox;
            //    delUpdate -= WaitTextRolling;
            //}
        }

        bool isAddMovingTextM = false;

        float GetRollingTime(float cHeight)
        {
            // 하이트의 70을 1초동안 움직이니
            // 그 값을 얻어내기 위해 cHeight값을 70으로 나누어서 시간을 추측해낸다.
            // 현재 임시로 70을 1초로 설정한 것이니 너무 느리거나 빠를경우 이 값을 변경해주어야 한다.
            // 그리고 0.5초를 더하여 텍스트가 사라질 때까지 롤링이 계속 되도록 하여 준다.
            // (0.5초는 텍스트 박스가 사라라지는 시간과 동일하다.- 바뀔경우 이 값도 바꾸어야 한다.)
            float getTime = (cHeight / scrollPerSec) + 0.5f;
            return getTime;
        }

        // 이부분은 델리게이트로 넣어서 업데이트 함수에 실행이 된다.
        // 기능을 모두 실행하면 델리게이트에서 제거하는 코드를 넣어줘야 한다.

        // 텍스트가 화면에 보여지는 시간을 계산하여 주는 부분이다.
        // 얼마의 시간동안 유지를 하고 있는 알 수 있도록 유지시간을 담는 변수를 하나 선언한다.

        float spendTime;
        void MovingText()
        {
            if (isRolling)
            {
                if (spendTime > textShowTime)
                {
                    if (isAddHideTextBoxM == false)
                    {
                        //Debug.Log("1Hide Text Box ___ Action Number ::: " + actionNum);
                        delUpdate += HideTextBox;
                        isAddHideTextBoxM = true;
                    }
                    else
                    {
                        //Debug.Log("이미 하이드텍스트박스 넣었다.");
                    }
                }
                spendTime += Time.deltaTime;
                if (spendTime > 1f)
                {
                    newPosY += Time.deltaTime * scrollPerSec;
                    conRectT.transform.localPosition = new Vector3(crp.x, newPosY, crp.z);
                }
            }
            else
            {
                if (spendTime < textShowTime)
                {
                    spendTime += Time.deltaTime;
                }
                else
                {
                    if (isAddHideTextBoxM == false)
                    {
                        //Debug.Log("2Hide Text Box ___ Action Number ::: " + actionNum);
                        delUpdate += HideTextBox;
                        isAddHideTextBoxM = true;
                    }
                    else
                    {
                        //Debug.Log("이미 하이드텍스트박스 넣었다.");
                    }
                }
            }
        }
        bool isAddHideTextBoxM = false;

        void HideTextBox()
        {
            // 2f를 곱한 것은 두배 빠르게 알파의 변화를 얻기 위해서.
            // 결국 0.5초면 1의 값에 도달한다.
            // 최초값인 alphaval을 곱하는 것은.
            // (Time.deltaTime * 2f)의 값이 1이 되었을때 alphaVal과 같은 값이 되도록 하기 위해서
            alphaChanged -= (Time.deltaTime * 2f) * alphaVal;

            if (alphaChanged > 0)
            {
                baseImage.color = ChangeAlpha(alphaChanged, baseImage.color);
                bubbleText.color = ChangeAlpha(alphaChanged, bubbleText.color);
            }
            else
            {
                // 텍스트 박스의 알파 애니를 끝낸다.
                // (원래 값을 넣는다)
                alphaChanged = 0f;
                baseImage.color = ChangeAlpha(0, baseImage.color);
                bubbleText.color = ChangeAlpha(0, bubbleText.color);

                // 텍스트박스가 다 끝나면 이 텍스트을 호출해던 곳의 특정 함수를 실행하는.
                // 아래 델리게이트를 실행한다.
                if(delEnd != null) delEnd();

                // delegate로 들어가 있던 ShowTextBox를 제거한다.
                delUpdate -= HideTextBox;
                isAddHideTextBoxM = false;

                // delegate로 들어가 있던 MovingText 제거한다. 이제 더이상의 움직임은 없다.
                delUpdate -= MovingText;
                isAddMovingTextM = false;
                // 텍스트박스를 감춰준다.
                //Debug.Log("텍스트박스를 감춰준다.");
                gameObject.SetActive(false);
            }
        }

        void Update()
        {
            if (delUpdate != null) delUpdate();
        }
    }
}