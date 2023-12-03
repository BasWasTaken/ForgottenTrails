using Bas.Utility;
using DataService;
using Ink.Runtime;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ForgottenTrails.InkFacilitation.StoryController.InterfaceBroking;
using static ForgottenTrails.InkFacilitation.StoryController.TextProduction;

namespace ForgottenTrails.InkFacilitation
{
    /// <summary>
    /// <para>Behaviour for parsing content from <see cref="Story"/> files and passing it onto the appropriate monobehaviours.</para>
    /// </summary>
    public partial class StoryController : MonoSingleton<StoryController>
    {
        // Constants
        #region Constants
        private const string dataLabel = "BasicInkScript"; // NOTE:  isn't this ridiculous? if you'll be using this object as ink interface all the time, it should't itelf store particular data, you should have objets etc store data... else all will be under here, won't it? or will it just be settings?

        #endregion
        // Inspector Properties & Helpers
        #region Inspector Properties & Helpers

        [field: SerializeField, ReadOnly, BoxGroup("State Info")]
        public StackBasedStateMachine<StoryController> StateMachine { get; private set; }


        public TextAsset InkStoryAsset { get { return AssetManager.Instance.TextAsset; } set { AssetManager.Instance.TextAsset = value; } }

        [SerializeField, BoxGroup("Data"), ReadOnly]
        [Tooltip("View data object containing INK data.")]
        private StoryData _inkDataAsset = null;
        public StoryData InkDataAsset 
        { 
            get 
            {
                if (_inkDataAsset == null) 
                {
                    _inkDataAsset = CreateBlankData();
                }
                return _inkDataAsset;
            } 
            internal set 
            {
                _inkDataAsset = value; 
            } 
        }
        
        [Tooltip("Reset ink data in object. Note: does not remove data from file")]
        [Button("ResetInkData", EButtonEnableMode.Editor)]public void ResetInkDataButton() => ResetData();

        [Tooltip("Load data from disk and reset scene.")]
        [Button("ResetScene",EButtonEnableMode.Playmode)] public void ResetSceneButton() => ResetScene();

        [field:SerializeField, BoxGroup("TextProducer")]
        internal TextProduction TextProducer { get; set; }

        [field: SerializeField, BoxGroup("SetDresser")]
        internal SetDressing SetDresser { get; set; }

        [field: SerializeField, BoxGroup("InterfaceBroker")]
        internal InterfaceBroking InterfaceBroker { get; set; }


        [field: SerializeField, BoxGroup("SceneReferences")]
        internal TMPro.TMP_InputField InputField { get; set; }


        #endregion
        // Public Properties
        #region Public Properties
        public Story Story { get; private set; }
        #endregion
        // Private Properties
        #region Private Properties

        #region States
        internal SCDummyState dummyState = new(); // TODO: how to avoid having to make this from here?
        internal SCSuperState superState = new();
        internal SCProductionState productionState = new();
        internal SCFunctionState functionState = new();
        internal SCWaitingForInputState waitingForInputState = new();
        internal SCWaitingForChoiceState waitingForChoiceState = new();
        internal SCWaitingForContinueState waitingForContinueState = new();
        internal SCBookMenuState bookMenuState = new();
        internal SCSettingsState settingsState = new();
        internal SCDataState dataState = new();
        internal SCInventoryState inventoryState = new();
        internal SCMapState mapState = new();
        internal SCPartyState partyState = new();
        internal SCSavingState savingState = new();
        #endregion

        #endregion

        // LifeCycle Methods
        #region LifeCycle Methods
        override protected void Awake()
        {
            base.Awake();
           
            transform.localPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y); // NOTE: Why do I do this?
        
            //book = FindFirstObjectByType<Book>(); 
            SetDresser.Assign();
            TextProducer.Assign();
            InterfaceBroker.Assign();
            InitialiseStateMachine();
        }
        private void InitialiseStateMachine()
        {
            Debug.Log("Making state machine.");
            StateMachine = new
            (
            this,
            dummyState,
            superState,
            productionState,
            functionState,
            waitingForInputState,
            waitingForChoiceState,
            waitingForContinueState,
            savingState,
            bookMenuState,
            settingsState,
            dataState,
            inventoryState,
            partyState,
            mapState
            );
        }
        private void Update()
        {
            if (StateMachine.CurrentState != null)
            {
                StateMachine.Update();
            }
        }
        private void OnDestroy()
        {
            InkDataAsset = null;
        }
        #endregion
        // Public Methods
        #region Public Methods

        #endregion
        // Private Methods
        #region Private Methods 
        /// <summary>
        /// Make a new <see cref="StoryData"/>object
        /// </summary>
        /// <returns>The freshly made blank data</returns>
        private StoryData CreateBlankData(bool forBootup = false)
        {
            // NOTE: Is this the optimal way of doing this?
            StoryData data = new();
            if (!forBootup) 
            { 
                Debug.Log("Created new data " + data.Label); 
            }
            return data;
        }
        private void ResetData()
        {
            InkDataAsset = CreateBlankData();
        }
        private void ResetScene()
        {
            if (UnityEditor.EditorApplication.isPlaying == true)
            {
                StopScene(); // should this be done from the statemachine???
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //StateMachine.Reset();
                //StateMachine.TransitionToState(StoryController.Instance.superState);
            }
        }
        private void StopScene()
        {
            foreach (GameObject obj in FindObjectsOfType<GameObject>())
            {
                StopAllCoroutines();
            }
        }
        private void PromptName()
        {
            StateMachine.TransitionToState(waitingForInputState);
            InputField.gameObject.SetActive(true);
            InputField.ActivateInputField();        
        }
        public void AssignName()
        {
            Story.state.variablesState["PlayerName"] = DataManager.Instance.MetaData.playerName = InputField.text;
            InputField.DeactivateInputField();
            InputField.gameObject.SetActive(false);
            waitingForInputState.DropCondition = true;
        }

        private void ConsoleLogInk(string text, bool warning = false)
        {
            if (warning)
            {
                Debug.LogWarning("Warning from INK Script: " + text);
            }
            else
            {
                Debug.Log("Message from INK Script: " + text);
            }
        }
        #endregion
    }
}