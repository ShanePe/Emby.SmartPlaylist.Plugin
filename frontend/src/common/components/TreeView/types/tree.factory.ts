import { TreeNodeData, TreeViewData } from '~/common/components/TreeView/types/tree';
import { Guid } from '~/common/helpers/guid';
import { normalizeArray } from '~/common/helpers/array';

export const createTreeNodeData = (template?: Partial<TreeNodeData>): TreeNodeData => {
    const newTreeNodeData = {
        id: Guid.newGuid(),
        children: [],
        isRoot: false,
        isExpanded: true,
        level: 0,
    };
    return { ...newTreeNodeData, ...template };
};

export const createTreeViewData = (nodes: TreeNodeData[]): TreeViewData => {
    const rootNodes = nodes.filter(x => x.isRoot);

    return {
        byId: normalizeArray(nodes, 'id'),
        rootIds: rootNodes.map(x => x.id)
    };
};
