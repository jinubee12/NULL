using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;





namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        public void ChangeScene(string name)
        {
            SceneManager.LoadScene(name);
        }

        [SerializeField] private InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private ScrollRect scroll;
        
        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        private float height;
        private OpenAIApi openai = new OpenAIApi();

        private List<ChatMessage> messages = new List<ChatMessage>();
        private string prompt;
        private int check =0;

        public string fHP = "";
        public string fSPEED = "";
        public string fSTRONG = "";
        public string fDEX = "";
        public string fPATTERN = "";

        public int fHPin = 1;
        public int fSPEEDin = 1;
        public int fSTRONGin = 1;
        public int fDEXin = 1;




        private void Start()
        {
            button.onClick.AddListener(SendReply);
            LoadPromptFromFile("prompt.txt"); //프롬프트 파일읽어오기

                ///초기메시지 띄우기
                var initialMessage = new ChatMessage()
                {
                 Role = "AI",
                 Content = "What kind of quest do you want to create?"
                };
                AppendMessage(initialMessage);
                ///////////////////

        }

        //프롬프트 파일을 읽어 오기위한 함수
        private void LoadPromptFromFile(string fileName)
        {
            string filePath = Path.Combine(Application.dataPath, fileName);

            if (File.Exists(filePath))
            {
                prompt = File.ReadAllText(filePath);
            }
            else
            {
                Debug.LogError("File not found: " + filePath);
                prompt = "Default prompt if file not found or can't be read.";
            }
        }
        /////////////////////////////////////
        ///


        ///

        //이미지파일 저장함수
        private void SaveImageToFile(Texture2D texture)
        {
            // Texture2D를 바이트 배열로 변환
            byte[] bytes = texture.EncodeToPNG();

            // 파일 경로 설정 (원하는 경로로 변경 가능)
            string filePath = Application.dataPath +"/Resources" + "/generated_image.png";

            // 파일 쓰기
            File.WriteAllBytes(filePath, bytes);

            Debug.Log("이미지가 성공적으로 저장되었습니다. 파일 경로: " + filePath);
        }

        //////////////////////////////////////

        private void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
        }

        private async void SendReply()
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = inputField.text
            };
            
            AppendMessage(newMessage);

            if (messages.Count == 0) {
                newMessage.Content = prompt + "\n" + inputField.text; 
            }
            else {
                newMessage.Content = "Please recommend monster attributes that match my current information and requirements for each attribute: [HP, SPEED, STRONG, DEX, Attack Pattern]. Provide the information in the format of [hpX, sX, stX, dX, Attack Pattern]. For example, provide the output in the format like [hp4, s2, st3, d4, DASH]. Now, I'll input the additional requirements from the user." + "\n" + inputField.text; 
            }
            
            messages.Add(newMessage);
            
            button.enabled = false;
            inputField.text = "";
            inputField.enabled = false;
            
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();            
                messages.Add(message);

                /////////////foundString에 생성하고자 하는 몬스터 아이디 저장//////////////
                var result = message.Content;


                for (int i = 0; i <= 100; i++)
                {
                    string searchString1 = "hp" + i.ToString();
                    if (result.Contains(searchString1))
                    {
                        //Debug.Log("Found: " + searchString1);
                        fHP = searchString1; 

                        string temp = searchString1.Substring(2);
                        fHPin = int.Parse(temp);
                        //Debug.Log(fHPin);
                        break;
                    }
                    
                }            

                
                for (int i = 0; i <= 10; i++)
                {
                    string searchString2 = "s" + i.ToString();
                    if (result.Contains(searchString2))
                    {
                        //Debug.Log("Found: " + searchString2);
                        fSPEED = searchString2;

                        string temp = searchString2.Substring(1);
                        fSPEEDin = int.Parse(temp);
                        //Debug.Log(fSPEEDin);
                        break;
                    }
                    
                }            

                
                for (int i = 0; i <= 50; i++)
                {
                    string searchString3 = "st" + i.ToString();
                    if (result.Contains(searchString3))
                    {
                        //Debug.Log("Found: " + searchString3);
                        fSTRONG = searchString3;

                        string temp = searchString3.Substring(2);
                        fSTRONGin = int.Parse(temp);
                        //Debug.Log(fSTRONGin);
                        break;
                    }
                    
                }            

                
                for (int i = 0; i <= 20; i++)
                {
                    string searchString4 = "d" + i.ToString();
                    if (result.Contains(searchString4))
                    {
                        //Debug.Log("Found: " + searchString4);
                        fDEX = searchString4;

                        string temp = searchString4.Substring(1);
                        fDEXin = int.Parse(temp);
                        //Debug.Log(fDEXin);
                        break;
                    }
                    
                }       

                string searchString5 = "DASH";
                if (result.Contains(searchString5))
                {
                    fPATTERN = searchString5;
                }    
                string searchString6 = "FLYATTACK";
                if (result.Contains(searchString6))
                {
                    fPATTERN = searchString6;
                }
                string searchString7 = "MISSILEATTACK";
                if (result.Contains(searchString7)) 
                {
                    fPATTERN = searchString7;
                }

                ////////////////////////////////////////////////////////////////////////////////

                //Debug.Log("Found string: " + foundString);
                Debug.Log("OK.\n" + "The information of the generated monster is" + fHP +" "+fSPEED +" "+fSTRONG +" "+fDEX +" "+fPATTERN +" " ); //콘솔창에는 GPT로 받은 결과 그대로 출력

                ///////////최종결정 몬스터 화면출력 - 지정된 형태로 출력////////////////
                message.Content = result;
                //AppendMessage(message);

                check = check +1;
                //////////////////////////////////////////////

                ///메시지 띄우기
                var me1 = new ChatMessage()
                {
                 Role = "AI",
                 Content = "I will provide you with recommended monster information."
                };
                AppendMessage(me1);
                ///////////////////

                //////////////////////gpt에게 다시물어봐서 몬스터 정보 출력하기////////////////////////////
                var newMessage2 = new ChatMessage()
                {
                Role = "user",
                Content = "Please describe this monster without directly specifying the range of each attribute."
                };

                messages.Add(newMessage2);

                var completionResponse2 = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
                {
                Model = "gpt-3.5-turbo-0613",
                Messages = messages
                });            
                message = completionResponse2.Choices[0].Message;
                message.Content = message.Content.Trim();            
                messages.Add(message);
                AppendMessage(message);
                ////////////////////////////////////////

                ///메시지 띄우기
                var me2 = new ChatMessage()
                {
                 Role = "AI",
                 Content = "If you want to create a monster, please press the generate button. If you want a different monster, please provide new requirements."
                };
                AppendMessage(me2);
                ///////////////////
                ///

                Debug.Log(message.Content);


                //////이미지생성///////
                var response = await openai.CreateImage(new CreateImageRequest
                {
                    Prompt = "make an monster image. please do not include text in image" + inputField.text,
                    Size = ImageSize.Size256
                });


                using (var request = new UnityWebRequest(response.Data[0].Url))
                {
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Access-Control-Allow-Origin", "*");
                    request.SendWebRequest();

                    while (!request.isDone) await Task.Yield();

                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(request.downloadHandler.data);
                    var sprite = Sprite.Create(texture, new Rect(0, 0, 256, 256), Vector2.zero, 1f);
                    //image.sprite = sprite;


                    ///////////////////////////////////////
                    SaveImageToFile(texture);
                    //////////////////////////////////////          

                }

                ///////////////////////////////

            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            button.enabled = true;
            inputField.enabled = true;




        }
    }
}
