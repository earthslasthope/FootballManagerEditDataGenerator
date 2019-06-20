import * as React from 'react';
import Context from './context.data-builder';

import NewItem from './new-item.data-builder';

import './styles.scss';

/** @type {GlobalTypes.MyAwesomeType} */
var obj = {
    name: 'bob'
};

obj.favoriteNumber = 'sfdsfdsfs';

/** @type {CrazyTypes.Nonesense} */
var crazy = {};
crazy.language = "English";

/** @type {CrazyTypes.Nonesense} */
var crazy2 = {
    isGay: 'No'
};

class DataBuilderMain extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            items: []
        };
    }

    render() {
        return <Context.Provider state={this.state}>
            <div className="data-builder">
                <NewItem />
            </div>
        </Context.Provider>
    }
}

export default DataBuilderMain;