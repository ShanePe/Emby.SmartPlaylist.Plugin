import * as React from 'react';
import { Override, useOverrides } from '~/common/hooks/useOverrides';
import { TreeNode, TreeNodeProps } from '~/common/components/TreeView/TreeNode';
import { TreeNodeData, TreeViewData } from '~/common/components/TreeView/types/tree';
import { Button } from '../Button';
import { Icon } from '../Icon';
import { TreeViewMultiAdd } from './TreeViewMultiAdd';
import { PlaylistContext } from '~/app/state/playlist/playlist.context';
import { explainPlaylistRules } from '~/emby/app.data';
import { getAppPlaylistForPlaylist } from '~/app/state/app.selectors';
import { showError } from '~/common/helpers/utils';
import { Modal } from '~/emby/components/Modal';
import { HierarchyStringContainer } from '../HierarchyStringContainer';
import './TreeView.css';

type TreeViewProps = {
    data: TreeViewData;
    overrides?: {
        Node: Override<TreeNodeProps>;
    };
    onExpandedChange?(nodeData: TreeNodeData, isExpanded: boolean): void;
    renderNodeContent(data: TreeNodeData): React.ReactNode;
};

export const TreeView: React.FC<TreeViewProps> = props => {
    const playlistContext = React.useContext(PlaylistContext);
    playlistContext.getPlaylist()
    let [showMultiAdd, setShowMultiAdd] = React.useState(false);
    let [explainedText, setExplainedText] = React.useState(undefined);
    const [Node, nodeProps] = useOverrides(props.overrides && props.overrides.Node, TreeNode);

    const { data } = props;
    const getRootNodes = (): TreeNodeData[] => {
        return data.rootIds.map(x => data.byId[x]);
    };

    const getChildNodes = (nodeData: TreeNodeData): TreeNodeData[] => {
        return nodeData.children.map(x => data.byId[x]);
    };

    const setExpanded = (nodeData: TreeNodeData, isExpanded: boolean): void => {
        props.onExpandedChange(nodeData, isExpanded);
    };

    return (
        <>
            <div className='tree-button-container' >
                <div className='tree-button-inner'>
                    <Button onClick={_ => explainPlaylistRules(getAppPlaylistForPlaylist(playlistContext.getPlaylist()))
                        .then(res => {
                            if (res.success) {
                                setExplainedText(res.response);
                            } else {
                                showError({ label: "Error", content: res.error, modal: true, timeout: 3000 });
                            }
                        })} title="Show a more human readable (hopefully) format of the rules.">
                        <Icon type="help" />
                    </Button>
                    <Button onClick={_ => setShowMultiAdd(true)} title='Added multiple values to a field'>
                        <Icon type="library_add" />
                    </Button>
                </div>
            </div>
            <div>
                {showMultiAdd && (
                    <TreeViewMultiAdd
                        onClose={() => setShowMultiAdd(false)}
                        onConfirm={(rules) => {
                            var node = playlistContext.getLastTreeNode();
                            if (node)
                                for (let i = rules.length - 1; i >= 0; i--) {
                                    playlistContext.addRuleEntity(node, rules[i]);
                                }
                            setShowMultiAdd(false);
                        }}
                    />
                )}
                {explainedText !== undefined && (
                    <Modal
                        onClose={() => { setExplainedText(undefined) }}
                        onConfirm={() => { setExplainedText(undefined) }}
                        confirmLabel='Thanks for that'
                        title='Mostest readable'
                        small={true}
                    >
                        <HierarchyStringContainer
                            value={explainedText}
                        />

                    </Modal>
                )}
            </div>
            {getRootNodes().map(nodeData => (
                <Node
                    key={nodeData.id}
                    data={nodeData}
                    getChildNodes={getChildNodes}
                    setExpanded={setExpanded}
                    renderContent={props.renderNodeContent}
                    {...nodeProps}
                />
            ))}
        </>
    );
};
