import * as React from 'react';
import * as wikiApi from '../../api/wikipedia';

import MainContext from './context.data-builder';

const NewItemContext = React.createContext({
    selectTeam: (teamName) => null,
    searchString: '',
    searchResults: null
});

const ReviewerApproval = () => <MainContext.Consumer>
        {({addItem}) =>
    <NewItemContext.Consumer>
        {({backToSearchResults, clear, reviewData}) => <div className="reviewer-approval">
            <span>Would you like to add the following team to your league?</span>
            <button type="button" className="btn yes" onClick={e => { addItem(reviewData); clear(); }}>Yes</button>
            <button type="button" className="btn no" onClick={e => { backToSearchResults(); }}>No</button>
        </div>}
    </NewItemContext.Consumer>
        }
</MainContext.Consumer>

const Reviewer = () => <NewItemContext.Consumer>
    {({reviewData}) => <div className="reviewer">
        {Object.keys(reviewData).map(key => reviewData[key]).map(({ property, data }) => <div key={property} className="review-item">
            <span className="property">{property}</span>
            <div className="data">
                {data.map(({ text, link, yearFrom, yearTo }, index) => <span key={index} className="value-item">
                    {text}
                </span>)}
            </div>
        </div>)}
        <ReviewerApproval />
    </div> }
</NewItemContext.Consumer>

const SearchResults = () => <NewItemContext.Consumer>
    {({searchResults, searchString, selectTeam}) => <div className="search-results">
        <span>Found matches for <span className="searchmatch">{searchString}</span></span>
        <ul>
            {searchResults.map(({pageTitle, snippetHtml}) => <li key={pageTitle} onClick={selectTeam.bind(this, pageTitle)}>
                <h6>{pageTitle}</h6>
                <p dangerouslySetInnerHTML={{ __html: snippetHtml }}></p>
            </li>)}
        </ul>
    </div> }
</NewItemContext.Consumer>

class NewItem extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            input: '',
            searchString: '',
            searchResults: null,
            reviewData: null
        };

        this.submit = this.submit.bind(this);
        this.selectTeam = this.selectTeam.bind(this);
        this.backToSearchResults = this.backToSearchResults.bind(this);
        this.clear = this.clear.bind(this);
    }

    submit(e) {
        e.preventDefault();
        const { input } = this.state;

        wikiApi.search(this.state.input).then(searchResults => {
            this.setState({
                searchResults,
                searchString: input,
                input: ''
            });
        });
    }

    selectTeam(teamName) {
        wikiApi.getInfobox(teamName).then(reviewData => {
            this.setState({
                reviewData,
                searchString: ''
            });
        });
    }

    backToSearchResults() {
        this.setState({
            reviewData: null
        });
    }

    clear() {
        this.setState({
            reviewData: null,
            searchResults: null
        });
    }

    render() {
        const { selectTeam, backToSearchResults, clear } = this;
        const { input, searchResults, searchString, reviewData } = this.state;

        return <NewItemContext.Provider value={{ selectTeam, searchResults, searchString, reviewData, backToSearchResults, clear }}>
            <div className="new-item">
                <p>Enter name of team to search and review</p>
                <form onSubmit={this.submit}>
                    <input type="text" onChange={(e) => this.setState({ input: e.target.value })} value={input} />
                    <button type="submit" disabled={input.length === 0}>Search</button>
                </form>
                { !reviewData && searchResults && <SearchResults /> }
                { reviewData && <Reviewer /> }
            </div>
        </NewItemContext.Provider>
    }
}

export default NewItem;