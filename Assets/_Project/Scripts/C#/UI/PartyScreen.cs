using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VVGames.ForgottenTrails.InkConnections;

//using VVGames.ForgottenTrails.InkConnections.Party;

namespace VVGames.ForgottenTrails.UI
{
    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    public class PartyScreen : MonoBehaviour
    {
        #region Fields

        public Book book;

        [Header("Prefab")]
        [SerializeField]
        private Button PartyMemberButton; // ToDO: replace with container holding scriptable object so you can include a portrait, etc.

        #endregion Fields

        #region Public Methods

        public void FetchPartyMembers(InkList inkParty)
        {
            Debug.Log(Time.time);
            // destroy all buttons
            foreach (Button button in GetComponentsInChildren<Button>())
            {
                Destroy(button.gameObject);
            }

            Debug.Log(Time.time);
            // make new

            foreach (InkListItem member in inkParty.Keys)
            {
                AddPartyMember(member);
            }
            Debug.Log(Time.time);
        }

        public void AddPartyMember(InkListItem member)
        {
            Button obj = Instantiate(PartyMemberButton, transform);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = member.itemName;
            obj.onClick.AddListener(() => StoryController.Instance.InterfaceBroker.TryConverseMember(member));
        }

        #endregion Public Methods
    }
}