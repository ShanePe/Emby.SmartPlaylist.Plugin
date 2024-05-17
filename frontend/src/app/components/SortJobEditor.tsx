import * as React from 'react';
import { Button } from '~/common/components/Button';
import { Icon } from '~/common/components/Icon';
import { InfoRow } from "~/common/components/InfoRow";
import { Inline } from "~/common/components/Inline";;
import { Select } from "~/common/components/Select";
import { Toggle } from "~/common/components/Toggle";
import { UpdateTypes } from "../app.const";
import { AppContext } from "../state/app.context";
import { PlaylistContext } from "../state/playlist/playlist.context";
import './SortJobEditor.css'
type SortJobEditorProps = {};

export const SortJobEditor: React.FC<SortJobEditorProps> = () => {
    const playlistContext = React.useContext(PlaylistContext);
    const appContext = React.useContext(AppContext);
    const { updateBasicData } = playlistContext;
    const basicData = playlistContext.getBasicData();
    const OrdersBy = appContext.getOrdersBy();

    return (
        <>
            <Inline>
                <Toggle
                    id="Sort-Enabled"
                    label='Enabled:'
                    checked={basicData.sortJob.enabled}
                    onToggled={c => {
                        updateBasicData({
                            sortJob: {
                                ...basicData.sortJob,
                                enabled: c
                            }
                        })

                    }} />
                <Select
                    label='Sort by:'
                    disabled={!basicData.sortJob.enabled}
                    maxWidth={true}
                    values={OrdersBy}
                    value={basicData.sortJob.orderBy}
                    onChange={newVal => {
                        var thenBys = basicData.sortJob.thenBys === undefined ? [] : basicData.sortJob.thenBys.filter(x => x !== newVal);

                        updateBasicData({
                            sortJob: {
                                ...basicData.sortJob,
                                orderBy: newVal,
                                thenBys: thenBys
                            },
                        })
                    }
                    }
                />
                <Select
                    label="Sort Trigger:"
                    disabled={!basicData.sortJob.enabled}
                    values={UpdateTypes.filter(x => x === 'Daily' || x === 'Weekly' || x === 'Monthly').map(x => x)}
                    value={basicData.sortJob.updateType}
                    onChange={newVal =>
                        updateBasicData({
                            sortJob: {
                                ...basicData.sortJob,
                                updateType: newVal
                            }
                        })
                    }
                    style={{ width: '120px' }}
                />
            </Inline>
            {basicData.sortJob.enabled && (
                <>
                    <Inline>
                        Then by:
                    </Inline>
                    <div className='thenby-container'>
                        <div className='thenby-container'>
                            {basicData.sortJob.thenBys?.map((thenBy, i) =>
                                <div className='thenby-container'>
                                    <Select
                                        key={"thenBy_" + i}
                                        value={thenBy}
                                        values={OrdersBy.filter(order => order === thenBy ||
                                            !(basicData.sortJob.orderBy === order || basicData.sortJob.thenBys?.includes(order)))}
                                        onChange={newVal => {
                                            var thenArray = basicData.sortJob.thenBys;
                                            thenArray[i] = newVal;

                                            updateBasicData({
                                                sortJob: {
                                                    ...basicData.sortJob,
                                                    thenBys: thenArray
                                                }
                                            })
                                        }}
                                    />
                                    <Button onClick={_ => {
                                        var thenArray = basicData.sortJob.thenBys.length === 1 ? [] : basicData.sortJob.thenBys.splice(i)

                                        updateBasicData({
                                            sortJob: {
                                                ...basicData.sortJob,
                                                thenBys: thenArray
                                            }
                                        })
                                    }}>
                                        <Icon type="remove" />
                                    </Button>
                                </div>
                            )}
                        </div>
                        <div className='thenby-container'>
                            <Button onClick={_ => {
                                var thenArray = basicData.sortJob.thenBys == undefined ? [] : basicData.sortJob.thenBys;

                                thenArray.push(OrdersBy.find(x => !thenArray.includes(x)));

                                updateBasicData({
                                    sortJob: {
                                        ...basicData.sortJob,
                                        thenBys: thenArray
                                    }
                                })
                            }}>
                                <Icon type="add" />
                            </Button>
                        </div>
                    </div>
                </>
            )
            }
            {
                basicData.sortJob.enabled && (
                    <>
                        <InfoRow InfoItems={[
                            { label: 'Last Run Duration: ', text: basicData.sortJob.lastDurationStr ? basicData.sortJob.lastDurationStr : 'N/A', visible: true },
                            { label: 'Runs: ', text: basicData.sortJob.syncCount ? basicData.sortJob.syncCount.toString() : 'N/A', visible: true },
                            { label: 'Last Updated: ', text: basicData.sortJob.lastUpdated ? basicData.sortJob.lastUpdated.toLocaleString() : 'N/A', visible: true }
                        ]} />
                        <InfoRow InfoItems={[
                            { label: 'Last Ran: ', text: basicData.sortJob.lastRan ? basicData.sortJob.lastRan.toLocaleString() : 'N/A', visible: true },
                            { label: 'Next Run: ', text: basicData.sortJob.nextUpdate ? basicData.sortJob.nextUpdate.toLocaleString() : 'N/A', visible: true }

                        ]} />
                        <InfoRow InfoItems={[
                            { label: 'Status: ', text: basicData.sortJob.status ? basicData.sortJob.status : 'N/A', visible: true }
                        ]} />
                    </>
                )
            }
        </>
    )
}