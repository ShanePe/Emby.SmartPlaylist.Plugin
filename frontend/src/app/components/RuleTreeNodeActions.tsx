import * as React from 'react';
import { Button } from '~/common/components/Button';
import { Icon } from '~/common/components/Icon';
import { PlaylistContext } from '~/app/state/playlist/playlist.context';
import { TreeNodeData } from '~/common/components/TreeView/types/tree';

export type RuleTreeNodeActionsProps = {
    node: TreeNodeData;
};

export const RuleTreeNodeActions: React.FC<RuleTreeNodeActionsProps> = props => {
    const { node } = props;
    const playlistContext = React.useContext(PlaylistContext);
    const { isRemovable, removeRule, addRule, addRuleGroup, canAddRule } = playlistContext;

    return (
        <div style={{ padding: '4px', display: 'inline-flex' }}>
            {canAddRule(node) && (
                <>
                    <Button disabled={!isRemovable(node)} onClick={_ => removeRule(node)}>
                        <Icon type="remove" />
                    </Button>

                    <Button onClick={_ => addRule(node)}>
                        <Icon type="add" />
                    </Button>

                    <Button onClick={_ => addRuleGroup(node)}>
                        <Icon type="group" />
                    </Button>
                </>
            )}
        </div>
    );
};
