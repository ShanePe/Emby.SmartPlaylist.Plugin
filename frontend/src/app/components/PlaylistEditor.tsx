import * as React from 'react';
import { PlaylistContext } from '~/app/state/playlist/playlist.context';
import { Select } from '~/common/components/Select';
import { TreeView } from '~/common/components/TreeView/TreeView';
import { RuleTreeNodeContent } from '~/app/components/RuleTreeNodeContent';
import { Input } from '~/common/components/Input';
import { AppContext } from '~/app/state/app.context';
import { defaultGroupMatchType, RuleMatchTypes, SmartTypes, UpdateTypes, CollectionModes, SourceTypes } from '~/app/app.const';
import { Inline } from '~/common/components/Inline';
import { TreeNodeData } from '~/common/components/TreeView/types/tree';
import { RuleOrRuleGroup } from '~/app/types/rule';
import { AutoSize } from '~/common/components/AutoSize';
import { Toggle } from '~/common/components/Toggle';
import Collapsible from 'react-collapsible';
import './PlaylistEditor.css'
import { TextArea } from '~/emby/components/TextArea';
import { showError } from '~/common/helpers/utils';
type PlaylistEditorProps = {};


export const PlaylistEditor: React.FC<PlaylistEditorProps> = () => {
    const playlistContext = React.useContext(PlaylistContext);
    const appContext = React.useContext(AppContext);
    const { updateBasicData, changeExpand, isShuffleUpdateType } = playlistContext;
    const rulesTree = playlistContext.getRulesTree();
    const basicData = playlistContext.getBasicData();
    const ordersBy = appContext.getOrdersBy();
    var sourceItems = appContext.getSourcesFor(basicData.sourceType);

    return (
        <>
            <Collapsible
                trigger="Setup"
                open={basicData.uiSections.setup}
                openedClassName='Window_look'
                contentOuterClassName={basicData.uiSections.setup ? '' : 'Hide_window'}
                onTriggerOpening={() => updateBasicData({
                    uiSections: {
                        ...basicData.uiSections,
                        setup: true
                    },
                })
                }
                onTriggerClosing={() => updateBasicData({
                    uiSections: {
                        ...basicData.uiSections,
                        setup: false
                    },
                })
                }

            >
                <Inline>
                    <Select
                        label="Type:"
                        values={SmartTypes.map(x => x)}
                        value={basicData.smartType}
                        onChange={newVal => {
                            updateBasicData({
                                smartType: newVal
                            })
                        }}
                        style={{ width: '120px' }}
                    />
                    {basicData.smartType === "Collection" && <Select
                        label='EpiMode:'
                        values={CollectionModes.map(x => x)}
                        value={basicData.collectionMode}
                        onChange={newVal =>
                            updateBasicData({
                                collectionMode: newVal
                            })
                        }
                        style={{ width: '100px' }}
                    />}
                    <Select
                        label='Source Type'
                        values={SourceTypes.map(x => x)}
                        value={basicData.sourceType}
                        onChange={newVal => {
                            sourceItems = appContext.getSourcesFor(newVal);
                            updateBasicData({
                                sourceType: newVal,
                                source: sourceItems[0],
                                monitorMode: false
                            });
                        }}
                        style={{ width: '150px' }}
                    />
                    {basicData.sourceType !== SourceTypes[0] && <Select
                        label='Source'
                        values={sourceItems.map(x => x.name)}
                        value={basicData.source.name}
                        onChange={newVal => updateBasicData({
                            source: sourceItems.filter(x => x.name === newVal)[0]
                        })}
                    />}
                    <Input
                        maxWidth={true}
                        value={basicData.name}
                        label="Name:"
                        onBlur={e =>
                            updateBasicData({
                                name: e.target.value,
                            })
                        }
                    />
                    <Select
                        label="Update type:"
                        values={UpdateTypes.map(x => x)}
                        value={basicData.updateType}
                        onChange={newVal => {
                            updateBasicData({
                                updateType: newVal,
                            });

                            if (!isShuffleUpdateType()) {
                                updateBasicData({
                                    monitorMode: false
                                })
                            }
                        }}
                        style={{ width: '120px' }}
                    />
                    {(isShuffleUpdateType()) && (
                        <Toggle
                            id='Toggle-Monitor-Mode'
                            label='Monitor'
                            checked={basicData.monitorMode}
                            labelTooltip='For shuffle modes, monitor will monitor library changes and include/exclude items between scheduled rebuilds. (Performance hit)'
                            onToggled={c => {
                                updateBasicData({
                                    monitorMode: c
                                })
                            }} />
                    )}
                </Inline>
            </Collapsible>
            <Collapsible
                trigger="Sorting"
                open={basicData.uiSections.sort}
                openedClassName='Window_look'
                contentOuterClassName={basicData.uiSections.sort ? '' : 'Hide_window'}
                onTriggerOpening={() => updateBasicData({
                    uiSections: {
                        ...basicData.uiSections,
                        sort: true
                    },
                })
                }
                onTriggerClosing={() => updateBasicData({
                    uiSections: {
                        ...basicData.uiSections,
                        sort: false
                    },
                })
                }
            >
                <Inline>
                    {(basicData.smartType === "Playlist" && basicData.updateType !== 'Live') && (
                        <>
                            <Toggle
                                id="Toggle-Sort"
                                label='Sort by:'
                                checked={basicData.newItemOrder.hasSort}
                                labelTooltip={isShuffleUpdateType() ? 'Sort all items by' : 'Sort newly added items by'}
                                onToggled={c => {
                                    updateBasicData({
                                        newItemOrder: {
                                            ...basicData.newItemOrder,
                                            hasSort: c,
                                            orderBy: ordersBy[0]
                                        }
                                    })
                                    if (c)
                                        updateBasicData({
                                            limit: {
                                                ...basicData.limit,
                                                hasLimit: false
                                            }
                                        })

                                }}
                            />
                            <Select
                                label={isShuffleUpdateType() ? 'Sort all items by:' : 'Sort newly added items by:'}
                                disabled={!basicData.newItemOrder.hasSort}
                                maxWidth={true}
                                values={ordersBy}
                                value={basicData.newItemOrder.orderBy}
                                onChange={newVal =>
                                    updateBasicData({
                                        newItemOrder: {
                                            ...basicData.newItemOrder,
                                            orderBy: newVal,
                                        },
                                    })
                                }
                            />

                        </>
                    )}
                    <Toggle
                        id="Toggle-Limit"
                        label='Limited:'
                        checked={basicData.limit.hasLimit}
                        labelTooltip='Limit the amount if items ordered by the Limit Order'
                        onToggled={c => {
                            updateBasicData({
                                limit: {
                                    ...basicData.limit,
                                    hasLimit: c,
                                    orderBy: ordersBy[0]
                                }
                            })

                            if (c)
                                updateBasicData({
                                    newItemOrder: {
                                        ...basicData.newItemOrder,
                                        hasSort: false
                                    }
                                })

                        }}
                    />
                    <Input
                        disabled={!basicData.limit.hasLimit}
                        maxWidth={true}
                        value={basicData.limit.maxItems}
                        label="Max items:"
                        type="number"
                        onBlur={e =>
                            updateBasicData({
                                limit: {
                                    ...basicData.limit,
                                    maxItems: Number(e.target.value)
                                },
                            })
                        }
                    />
                    <Select
                        disabled={!basicData.limit.hasLimit}
                        maxWidth={true}
                        values={ordersBy}
                        label="Limit Order:"
                        value={basicData.limit.orderBy}
                        onChange={newVal =>
                            updateBasicData({
                                limit: {
                                    ...basicData.limit,
                                    orderBy: newVal,
                                },
                            })
                        }
                    />
                </Inline>
            </Collapsible>
            <Collapsible
                trigger="Rules"
                open={basicData.uiSections.rules}
                openedClassName='Window_look'
                contentOuterClassName={basicData.uiSections.rules ? '' : 'Hide_window'}
                onTriggerOpening={() => updateBasicData({
                    uiSections: {
                        ...basicData.uiSections,
                        rules: true
                    },
                })
                }
                onTriggerClosing={() => updateBasicData({
                    uiSections: {
                        ...basicData.uiSections,
                        rules: false
                    },
                })
                }
            >
                <TreeView
                    data={rulesTree}
                    renderNodeContent={nodeData => <RuleTreeNodeContent node={nodeData} />}
                    onExpandedChange={(node, isExpanded) => changeExpand(node, isExpanded)}
                    overrides={{
                        Node: {
                            props: {
                                overrides: {
                                    ContentContainer: {
                                        props: {
                                            renderGroupHeader: nodeData => (
                                                <GroupContentHeader node={nodeData} />
                                            ),
                                        },
                                    },
                                },
                            },
                        },
                    }}
                />
            </Collapsible>
            <Collapsible
                trigger="Notes"
                open={basicData.uiSections.notes}
                openedClassName='Window_look'
                contentOuterClassName={basicData.uiSections.notes ? '' : 'Hide_window'}
                onTriggerOpening={() => updateBasicData({
                    uiSections: {
                        ...basicData.uiSections,
                        notes: true
                    },
                })
                }
                onTriggerClosing={() => updateBasicData({
                    uiSections: {
                        ...basicData.uiSections,
                        notes: false
                    },
                })
                }
            >
                <TextArea
                    html={basicData.notes}
                    onValueChange={(html) => {
                        updateBasicData({
                            notes: html
                        })
                    }}
                    onError={(e) => {
                        showError({ label: "Error saving note", content: e, modal: true });
                    }}
                />
            </Collapsible>
        </>
    );
};

const GroupContentHeader: React.FC<{ node: TreeNodeData<RuleOrRuleGroup> }> = props => {
    const playlistContext = React.useContext(PlaylistContext);
    const nodeData = props.node.data;
    return (
        <>
            {nodeData.kind === 'ruleGroup' && (
                <AutoSize>
                    <Select
                        title='All items need to match under an "All" group but only one needs to match in an "Any" group for the item to be matched successfully'
                        values={RuleMatchTypes.map(x => x)}
                        value={nodeData.matchMode || defaultGroupMatchType}
                        onChange={newVal => playlistContext.changeMatchMode(nodeData, newVal)}
                    />
                </AutoSize>
            )}
        </>
    );
};
