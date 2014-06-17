using UnityEngine;
using System.Collections;
using PH.DataTree;

public class CardControls : MonoBehaviour {
	public TreeTest MyTree;
	public DTreeNode<CardModel> Node;

	void OnClick(){
		this.Node.Nodes.Add(new CardModel());
		this.MyTree.RefreshTree();
	}
}
