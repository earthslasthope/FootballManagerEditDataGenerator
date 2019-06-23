import * as React from 'react';
import Context from './context.data-builder';

import NewItem from './new-item.data-builder';

import './styles.scss';

class DataBuilderMain extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            items: []
        };

        this.addItem = this.addItem.bind(this);
    }

    addItem(item) {
        this.setState({
            items: [...this.state.items, item]
        });
    }

    render() {
        const { addItem, state } = this;
        const providerValue = {
            addItem,
            ...state
        };

        return <Context.Provider value={providerValue}>
            <div className="data-builder">
                <NewItem />
            </div>
        </Context.Provider>
    }
}

export default DataBuilderMain;