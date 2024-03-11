using Ink.Runtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VVGames.ForgottenTrails.InkConnections;
using VVGames.ForgottenTrails.InkConnections.Party;

namespace VVGames.ForgottenTrails.UI
{
    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    public class PartyScreen : MonoBehaviour
    {
        #region Fields

        [Header("Prefab")]
        [SerializeField]
        private Button PartyMemberButton;

        private Dictionary<InkListItem, PartyMemberContainer> UnityParty = new();

        #endregion Fields

        #region Public Methods

        public void Init()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }

        public void FetchPartyMembers(InkList inkParty)
        {
            // destroy all buttons
            for (int i = 0; i < UnityParty.Count; i++)
            {
                Destroy(UnityParty[UnityParty.Keys.ToArray()[i]].button.gameObject);
            }
            UnityParty.Clear();

            // make new

            foreach (InkListItem member in inkParty.Keys)
            {
                //Debug.Log(item);
                if (!UnityParty.ContainsKey(member))
                {
                    AddPartyMember(member);
                }
            }
        }

        public void AddPartyMember(InkListItem memberToAdd)
        {
            Debug.Log(memberToAdd.itemName);
            Debug.Log(memberToAdd);
            if (memberToAdd.itemName == "Player")
            {
                Debug.Log("Note: ik moet nog bedenken wat ik wil doen voor het speler karakter zelf. Player name inserten? Wait a minute, heeft het uberhaubt toegevoegde waarde om je eigen naam in te vullen?");
                return;
            }
            else if (AssetManager.Instance.PartyMemberDictionary.TryGetValue(memberToAdd, out PartyMemberSO memberFromAssets))
            {
                Debug.Log("Found " + memberToAdd);
                if (!UnityParty.ContainsKey(memberToAdd))
                {
                    Button obj = Instantiate(PartyMemberButton, transform);

                    obj.GetComponentInChildren<TextMeshProUGUI>().text = memberFromAssets.CanonicalName;
                    obj.onClick.AddListener(() => StoryController.Instance.InterfaceBroker.TryConverseMember(memberFromAssets));
                    obj.GetComponent<Image>().sprite = memberFromAssets.image;

                    UnityParty.Add(memberToAdd, new PartyMemberContainer(obj, memberFromAssets));
                }
                else
                {
                    //Debug.LogErrorFormat("item {0} already in party", item.itemName);
                }
            }
            else
            {
                Debug.LogError(string.Format("Member \"{0}\" not recognised!", memberToAdd.itemName));
            }
        }

        #endregion Public Methods
    }

    public class PartyMemberContainer
    {
        #region Fields

        public Button button;
        public PartyMemberSO definition;

        #endregion Fields

        #region Public Constructors

        public PartyMemberContainer(Button button, PartyMemberSO definition)
        {
            this.button = button;
            this.definition = definition;
        }

        #endregion Public Constructors
    }
}